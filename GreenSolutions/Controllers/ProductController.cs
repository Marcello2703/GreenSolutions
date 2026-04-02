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
    }
}
