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
    /// <summary>
    /// Controller for products in list.
    /// </summary>
    [Authorize]
    public class ProductsController : Controller {
        private readonly ProductListAppContext _context;

        public ProductsController(ProductListAppContext context) {
            _context = context;
        }

        /// <summary>
        /// Returns view with products in selected list.
        /// Also allows to add new products.
        /// </summary>
        /// <param name="listId">ID of selected list of products.</param>
        /// <param name="listName">Name of selected list of products.</param>
        /// <returns></returns>
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

        /// <summary>
        /// If data provided by user in form is correct, adds new product to the list.
        /// Otherwise displays corresponding error message.
        /// </summary>
        /// <param name="products">Data pulled from form.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(Products products) {
            products.NewProduct.Name = products.NewProduct.Name?.Trim();
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

        /// <summary>
        /// Displays details of selected product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {  return NotFound(); }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) { return NotFound(); }

            return View(product);
        }

        /// <summary>
        /// Redirects to view that allows to edit selected product.
        /// </summary>
        /// <param name="id">Selected product ID.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Submits edit of previously selected product.
        /// </summary>
        /// <param name="id">ID of selected product.</param>
        /// <param name="product">Data pulled from edit form.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Redirects to page that allows to delete selected product.
        /// </summary>
        /// <param name="id">Selected product ID.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Submits deletion of previously selected product.
        /// </summary>
        /// <param name="id">Selected product ID.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates checkbox to given value that is corresponding to status field of product identified by ID.
        /// </summary>
        /// <param name="id">ID of product.</param>
        /// <param name="isChecked">Checkbox value.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateCheckbox(int id, bool isChecked) {
            var item = await _context.Products.FindAsync(id);
            if (item != null) {
                item.Status = isChecked;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks of product with given ID exists
        /// </summary>
        /// <param name="id">ID of product</param>
        /// <returns></returns>
        private bool ProductExists(int id) {
            return _context.Products.Any(e => e.Id == id);
        }

        /// <summary>
        /// Returns list of products in product list with given ID.
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Product>> GetProducts(int? listID) {
            return await _context.Products.Where(o => o.ProductListId == listID).ToListAsync();
        }
    }
}
