using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using ProxyService.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplicationOrders.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductProxy _proxy;

        public ProductsController(IProductProxy proxy)
        {
            _proxy = proxy;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _proxy.GetAllAsync();
                if (products == null)
                {
                    // Si la lista es nula, retorna una lista vacía
                    products = new List<Product>();
                }
                return View(products);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, redirige a una vista de error
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,SupplierId,UnitPrice,Package,IsDiscontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proxy.CreateAsync(product);
                    if (result == null)
                    {
                        return RedirectToAction("Error", new { message = "El producto con el mismo nombre ya existe para el proveedor seleccionado." });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = ex.Message });
                }
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _proxy.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,SupplierId,UnitPrice,Package,IsDiscontinued")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proxy.UpdateAsync(id, product);
                    if (!result)
                    {
                        return RedirectToAction("Error", new { message = "No se puede realizar la edición porque hay duplicidad de nombre con otro producto del mismo proveedor." });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = ex.Message });
                }
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _proxy.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _proxy.DeleteAsync(id);
                if (!result)
                {
                    return RedirectToAction("Error", new { message = "No se puede eliminar el producto porque está asociado a pedidos." });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _proxy.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }

        // Error view handler
        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
