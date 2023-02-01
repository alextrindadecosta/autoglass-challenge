using System;
using System.Threading.Tasks;
using Autoglass.Domain.Entities;
using Autoglass.Domain.Interfaces;
using Autoglass.Domain.Models;
using Autoglass.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Autoglass.Application.Controllers
{
    [ApiController]
    [Route("v1")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        
        [HttpGet]
        [Route("supplier")]
        public async Task<IActionResult> Get(
            [FromQuery] string? description,
            [FromQuery] string? cnpj,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var suppliers = await _supplierService.GetAllAsync(description, cnpj, pageSize, pageNumber);
            
            return suppliers == null ? NoContent() : Ok(suppliers);
        }
        
        [HttpGet]
        [Route("suppliers/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            
            return supplier == null ? NotFound() : Ok(supplier);
        }
        
        [HttpPost("suppliers")]
        public async Task<IActionResult> PostAsync([FromBody] CreateSupplierViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var supplier = await _supplierService.RegisterAsync(model);

            return Created($"v1/suppliers/{supplier.Id}", supplier);
        }
        
        [HttpPut("suppliers")]
        public async Task<IActionResult> UpdateAsync(
            [FromBody] UpdateSupplierViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var supplier = await _supplierService.UpdateAsync(model);
            
            if (supplier == null)
            {
                return BadRequest("Não foi possível atualizar o produto.");
            }
            
            return Ok(supplier);
        }
    }
}