using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface ISupplierService
    {
        Task<ActionResult<Supplier>> CreateAsync([FromBody] Supplier toCreate);
        Task<IActionResult> DeleteAsync(int id);
        Task<ActionResult<List<Supplier>>> GetAll();
        Task<ActionResult<Supplier>> RetrieveAsync(int id);
        Task<IActionResult> UpdateAsync(int id, [FromBody] Supplier toUpdate);
    }
}
