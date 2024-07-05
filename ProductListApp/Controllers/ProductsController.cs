using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using OpenQA.Selenium.DevTools.V124.Audits;
using ProductListApp.Data;
using ProductListApp.Models;

namespace ProductListApp.Controllers {
    [Authorize]
    public class ProductsController : Controller {
        private readonly ProductListAppContext _context;

        public ProductsController(ProductListAppContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? listId, string? listName) {
            if (listId == 0 || listId == null) {
                listId = (int?)TempData["ProductListId"];
                listName = (string?)TempData["ProductListName"];

                TempData.Keep("ProductListId");
                TempData.Keep("ProductListName");
            }
            else {
                TempData["ProductListId"] = listId;
                TempData["ProductListName"] = listName;
            }

            return View(new Products(listName, new Product(), await GetProducts(listId)));
        }

        [HttpPost]
        public async Task<IActionResult> Index(Products products) {
            TempData.Keep("ProductListId");
            TempData.Keep("ProductListName");

            if (TempData["ProductListId"] is int listId) {
                products.NewProduct.ProductListId = listId;
            }
            else { return NotFound(); }
            string? productListName = (string?)TempData["ProductListName"];
            if (ModelState.IsValid) {
                _context.Add(products.NewProduct);
                await _context.SaveChangesAsync();

                return View(new Products(productListName, new Product(), await GetProducts(listId)));
            }
            products.ProductList = await GetProducts(listId);
            products.ListName = productListName;
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {  return NotFound(); }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) { return NotFound(); }

            return View(product);
        }

        //[HttpGet]
        //public IActionResult Create() {
        //    return RedirectToAction(nameof(Index));
        //    //return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Price,Quantity")] Product product) {
        //    if (ModelState.IsValid) {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
            
        //    TempData["Product"] = JsonSerializer.Serialize(product);

        //    return RedirectToAction(nameof(Index));
        //    //return View(product);
        //}

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                if (TempData["EditProductId"] is int tempId) {
                    id = tempId;
                    TempData.Keep("EditProductId");
                }
                else {
                    return NotFound();
                }
            }

            var product = await _context.Products.FindAsync(id);
            Console.WriteLine(JsonSerializer.Serialize(product));

            if (product == null) { return NotFound(); }

            TempData["EditProductId"] = id;
            TempData["EditProductListId"] = product.ProductListId;

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product) {
            if (id != product.Id) {
                return NotFound();
            }

            if (TempData["EditProductListId"] is not int editListId) {
                return NotFound();
            }

            product.ProductListId = editListId;

            if (ModelState.IsValid) {
                try {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ProductExists(product.Id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                if (TempData["DeleteProductId"] is int tempId) {
                    id = tempId;
                    TempData.Keep("DeleteProductId");
                }
                else {
                    return NotFound();
                }
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) { return NotFound(); }

            TempData["DeleteProductId"] = id;

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var product = await _context.Products.FindAsync(id);
            if (product != null) {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCheckbox(int id, bool isChecked) {
            var item = await _context.Products.FindAsync(id);
            if (item != null) {
                item.Status = isChecked;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id) {
            return _context.Products.Any(e => e.Id == id);
        }

        private async Task<IEnumerable<Product>> GetProducts(int? listID) {
            return await _context.Products.Where(o => o.ProductListId == listID).ToListAsync();
        }
    }
}
