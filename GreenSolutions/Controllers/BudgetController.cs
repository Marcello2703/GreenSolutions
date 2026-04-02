using GreenSolutions.DTOs;
using GreenSolutions.Models;
using GreenSolutions.Persistence;
using GreenSolutions.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenSolutions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PricingService _pricingService;
        public BudgetController(AppDbContext appDbContext, PricingService pricingService) 
        {
            _appDbContext = appDbContext;
            _pricingService = pricingService;
        }

        // GET: api/<BudgetController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<BudgetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BudgetController>
        [HttpPost]
        public async Task<IActionResult> CreateBudget(CreateBudgetDTO dto)
        {
            var user = await _appDbContext.UsersDB.FindAsync(dto.UserId);
            if(user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var client = await _appDbContext.ClientsDB.FindAsync(dto.ClientId);
            if (client == null)
            {
                return NotFound("Client não encontrado.");
            }

            var budget = new Budget(dto.UserId, user, dto.ClientId, client, new List<BudgetItem>());
            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = await _appDbContext.ProductsDB.FindAsync(item.ProductId);
                if (product == null)
                {
                    return NotFound($"Produto com ID {item.ProductId} não encontrado.");
                }
                var unitPrice = _pricingService.Calculate(product.BasePrice, client.ClientType);
                var totalPrice = unitPrice * item.Quantity;
                budget.Items.Add(new BudgetItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice
                });
                total += totalPrice;
            }

            budget.TotalPrice = total;

            _appDbContext.BudgetsDB.Add(budget);
            await _appDbContext.SaveChangesAsync();

            return Ok(budget);
        }

        // PUT api/<BudgetController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<BudgetController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
