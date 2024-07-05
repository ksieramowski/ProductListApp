using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using ProductListApp.Data;
using ProductListApp.Models;

namespace ProductListApp.Controllers {
    [Authorize]
    public class ProductListsController : Controller {

        private readonly ProductListAppContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductListsController(ProductListAppContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            User? user = await GetCurrentUserAsync();
            IEnumerable<ProductList>? productLists = await GetProductLists(user);
            if (productLists == null) { return NotFound(); }

            foreach (var productList in productLists) {
                var products = await _context.Products.Where(o => o.ProductListId == productList.Id).ToListAsync();
                int completed = 0;
                foreach (var product in products) {
                    if (product.Status == true) {
                        completed++;
                    }
                }
                productList.Status = $"({completed}/{products.Count()})";
            }

            return View(new ProductLists(new ProductList(), productLists));
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductLists productLists) {
            if (ModelState.IsValid) {
                User? user = await GetCurrentUserAsync();
                if (user == null) {
                    Console.WriteLine("Current user is null.");
                    return NotFound();
                }

                productLists.NewProductList.UserId = user.Id;
                productLists.NewProductList.CreationTime = DateTime.Now;
                _context.Add(productLists.NewProductList);
                
                await _context.SaveChangesAsync();

                var result = await GetProductLists(user);
                if (result == null) {
                    return NotFound();
                }

                return View(new ProductLists(new ProductList(), result));
            }
            else {
                User? user = await GetCurrentUserAsync();
                var result = await GetProductLists(user);
                if (result == null) {
                    return NotFound();
                }
                productLists.ListOfProductLists = result;
                return View(productLists);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                if (TempData["EditProductListId"] is int tempId) {
                    id = tempId;
                    TempData.Keep("EditProductListId");
                }
                else {
                    return NotFound();
                }
            }

            var productList = await _context.ProductLists.FindAsync(id);

            if (productList == null) { return NotFound(); }

            TempData["EditProductListId"] = id;
            TempData["EditProductListCreationTime"] = productList.CreationTime.ToString();

            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductList productList) {
            if (id != productList.Id) {
                return NotFound();
            }

            Console.WriteLine($"edit ID: {id}");

            User? user = await GetCurrentUserAsync();
            if (user == null) { return NotFound(); }
            productList.UserId = user.Id;

            Console.WriteLine($"UserID: '{productList.UserId}'");
            if (DateTime.TryParse((string?)TempData["EditProductListCreationTime"], out DateTime time)) {
                productList.CreationTime = time;
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(productList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ProductListExists(productList.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                if (TempData["DeleteProductListId"] is int tempId) {
                    id = tempId;
                    TempData.Keep("DeleteProductListId");
                }
                else {
                    return NotFound();
                }
            }

            var productList = await _context.ProductLists.FirstOrDefaultAsync(m => m.Id == id);

            if (productList == null) { return NotFound(); }

            TempData["DeleteProductListId"] = id;

            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var productList = await _context.ProductLists.FindAsync(id);
            if (productList != null) {
                _context.ProductLists.Remove(productList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductListExists(int id) {
            return _context.ProductLists.Any(e => e.Id == id);
        }

        private async Task<IEnumerable<ProductList>?> GetProductLists(User? user) {
            if (user == null) { return null; }
            return await _context.ProductLists.Where(o => o.UserId == user.Id).ToListAsync();
        }

        private async Task<User?> GetCurrentUserAsync() {
            if (_httpContextAccessor.HttpContext == null) { return null; }
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        }
    }
}
