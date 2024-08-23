using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using ProxyService.Interfaces;
using System.Threading.Tasks;
using ProxyService;

namespace WebApplicationOrders.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierProxy _proxy;

        public SuppliersController(ISupplierProxy proxy)
        {
            _proxy = proxy;
        }

        // GET: Supplier
        public async Task<IActionResult> Index()
        {
            var suppliers = await _proxy.GetAllAsync();
            return View(suppliers);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyName,ContactName,ContactTitle,City,Country,Phone,Fax")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                return await HandleActionAsync(async () =>
                {
                    var result = await _proxy.CreateAsync(supplier);
                    if (result == null)
                    {
                        return RedirectToAction("Error", new { message = "El proveedor con el mismo nombre ya existe." });
                    }
                    return RedirectToAction(nameof(Index));
                }, supplier);
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _proxy.GetByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,ContactName,ContactTitle,City,Country,Phone,Fax")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await HandleActionAsync(async () =>
                {
                    var result = await _proxy.UpdateAsync(id, supplier);
                    if (!result)
                    {
                        return RedirectToAction("Error", new { message = "No se puede realizar la edición porque hay duplicidad de nombre con otro proveedor." });
                    }
                    return RedirectToAction(nameof(Index));
                }, supplier);
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _proxy.GetByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var result = await _proxy.DeleteAsync(id);
                if (!result)
                {
                    return RedirectToAction("Error", new { message = "No se puede eliminar el proveedor porque tiene productos asociados." });
                }
                return RedirectToAction(nameof(Index));
            });
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _proxy.GetByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // Error view handler
        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

        private async Task<IActionResult> HandleActionAsync(Func<Task<IActionResult>> action, object model = null)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }
    }
}
