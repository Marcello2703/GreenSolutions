using GreenSolutions.Models;
using GreenSolutions.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenSolutions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public ProductController(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            _appDbContext.ProductsDB.Add(product);
            await _appDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet("getAllProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _appDbContext.ProductsDB.ToListAsync();
            if (!products.Any()) { return NotFound("Não existem produtos."); }

            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] DTOs.ProductDTOs.UpdateProductDTO updatedProduct)
        {
            var product = await _appDbContext.ProductsDB.FindAsync(id);
            if (product == null)
            {
                return NotFound("Produto não encontrado.");
            }

            product.Name = updatedProduct.Name;
            product.BasePrice = updatedProduct.BasePrice;

            await _appDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _appDbContext.ProductsDB.FindAsync(id);
            if (product == null)
            {
                return NotFound("Produto não encontrado.");
            }

            _appDbContext.ProductsDB.Remove(product);
            await _appDbContext.SaveChangesAsync();

            return Ok("Produto deletado.");
        }
    }
}
