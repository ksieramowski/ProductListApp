using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index() {
            return View(new Products(new Product(), await _context.Products.ToListAsync()));
        }

        [HttpPost]
        public async Task<IActionResult> Index(Products products) {
            if (ModelState.IsValid) {
                _context.Add(products.NewProduct);
                await _context.SaveChangesAsync();
                return View(new Products(new Product(), await _context.Products.ToListAsync()));
            }
            products.ProductList = await _context.Products.ToListAsync();
            Console.WriteLine(JsonSerializer.Serialize(products));
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) {
                return NotFound();
            }

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
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Quantity")] Product product) {
            if (id != product.Id) {
                return NotFound();
            }

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
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) {
                return NotFound();
            }

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

        private bool ProductExists(int id) {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
