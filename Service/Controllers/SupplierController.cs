using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Entities.Models;
using BLL.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly Suppliers _bll;

        public SupplierController(Suppliers bll)
        {
            _bll = bll;
        }

        // GET: api/Supplier
        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> GetAll()
        {
            try
            {
                var result = await _bll.RetrieveAllAsync();
                return Ok(result);
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // GET api/Supplier/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> RetrieveAsync(int id)
        {
            try
            {
                var supplier = await _bll.RetrieveByIDAsync(id);

                if (supplier == null)
                {
                    return NotFound("Supplier not found.");
                }

                return Ok(supplier);
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // POST: api/Supplier
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateAsync([FromBody] Supplier toCreate)
        {
            try
            {
                var supplier = await _bll.CreateAsync(toCreate);
                return CreatedAtRoute(nameof(RetrieveAsync), new { id = supplier.Id }, supplier);
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // PUT api/Supplier/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Supplier toUpdate)
        {
            toUpdate.Id = id;
            try
            {
                var result = await _bll.UpdateAsync(toUpdate);
                if (!result)
                {
                    return NotFound("Supplier not found or update failed.");
                }
                return NoContent();
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // DELETE api/Supplier/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _bll.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Supplier not found or deletion failed.");
                }
                return NoContent();
            }
            catch (SupplierExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
