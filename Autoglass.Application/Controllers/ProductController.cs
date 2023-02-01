using System.Threading.Tasks;
using Autoglass.Domain.Interfaces;
using Autoglass.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Autoglass.Application.Controllers
{
    [ApiController]
    [Route("v1")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> Get(
            [FromQuery] string? description,
            [FromQuery] int? supplier,
            [FromQuery] string? manufactureDate,
            [FromQuery] string? expirationDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var products = await _productService.GetAllAsync(
                description, 
                supplier, 
                manufactureDate, 
                expirationDate,
                pageSize,
                pageNumber);
            
            return products == null ? NoContent() : Ok(products);
        }
        
        [HttpGet]
        [Route("products/{id}")]
        public async Task<IActionResult> GetById(
            [FromRoute] int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost("products")]
        public async Task<IActionResult> PostAsync(
            [FromBody] CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var product = await _productService.RegisterAsync(model);
            
            if (product == null)
            {
                return BadRequest("Não foi possível salvar o produto.");
            }
            
            return Created($"v1/products/{product.Id}", product);
        }
        
        [HttpPut("products")]
        public async Task<IActionResult> UpdateAsync(
            [FromBody] UpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var product = await _productService.UpdateAsync(model);
            
            if (product == null)
            {
                return BadRequest("Não foi possível atualizar o produto.");
            }
            
            return Ok(product);
        }
        
        [HttpDelete("products")]
        public async Task<IActionResult> UpdateAsync(
            [FromQuery] int id)
        {
            var product = await _productService.RemoveAsync(id);
            
            if (product == null)
            {
                return BadRequest("Não foi possível remover o produto.");
            }
            
            return Ok(product);
        }
    }
}