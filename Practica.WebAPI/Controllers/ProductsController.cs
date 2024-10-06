
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practica.Application.Services;
using Practica.Domain.Entities;

namespace Practica.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        // GETByID: api/Products/id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductByID(int id) 
        {
                var product= await _productService.GetProductByIdAsync(id);
                return product == null ? NotFound() : Ok(product);
        }

        //Update Product
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductByID(int id, [FromBody] Product product)
        {
            try
            {
                await _productService.UpdateProductByIdAsync(id, product);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); 
            }
        }

        //Delete product
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductByID(int id) {
            try
            {
                await _productService.DeleteProductByIdAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException) {
                return NotFound();
            }        
        }
    }
}
