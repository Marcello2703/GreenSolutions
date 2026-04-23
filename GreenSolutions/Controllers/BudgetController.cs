using GreenSolutions.DTOs;
using GreenSolutions.DTOs.BudgetDTOs;
using GreenSolutions.Models;
using GreenSolutions.Persistence;
using GreenSolutions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenSolutions.Controllers
{
    [Route("api/budgets")]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBudgetById(int id)
        {
            var budget = await _appDbContext.BudgetsDB
                .Include(b => b.Client)
                .Include(b => b.Company)
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (budget == null)
                return NotFound("Orçamento não encontrado.");

            var budgetDTO = new BudgetResponseDTO
            {
                Id = budget.Id,
                CreatedAt = budget.CreatedAt,
                TotalPrice = budget.TotalPrice,
                ClientName = budget.Client.Name,
                CompanyName = budget.Company.Name,
                UserName = budget.User.Name,
                Items = budget.Items.Select(i => new BudgetItemResponseDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };

            return Ok(budgetDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgets()
        {
            var budgets = await _appDbContext.BudgetsDB
                .Include(b => b.Client)
                .Include(b => b.Company)
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var budgetsDTO = budgets.Select(b => new BudgetResponseDTO
            {
                Id = b.Id,
                UserName = b.User.Name,
                ClientName = b.Client.Name,
                CompanyName = b.Company.Name
            }).ToList();

            return Ok(budgetsDTO);
        }

        // POST api/<BudgetController>
        [HttpPost]
        public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDTO dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                return BadRequest("O orçamento precisa ter pelo menos um item.");

            if (dto.Items.Any(i => i.Quantity <= 0))
                return BadRequest("Todos os itens precisam ter quantidade maior que zero.");

            var user = await _appDbContext.UsersDB.FindAsync(dto.UserId);
            if(user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var client = await _appDbContext.ClientsDB.FindAsync(dto.ClientId);
            if (client == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var company = await _appDbContext.CompaniesDB.FindAsync(dto.CompanyId);
            if(company == null)
            {
                return NotFound("Empresa não encontrada.");
            }

            //Recupera os produtos do orçamento para calcular os preços
            var productIds = dto.Items.Select(i => i.ProductId).ToList();
            var products = await _appDbContext.ProductsDB.Where(p => productIds.Contains(p.Id)).ToListAsync();

            var budget = new Budget(dto.UserId, user, dto.ClientId, client, dto.CompanyId, company, new List<BudgetItem>());
            var budget2 = new Budget
            {
                UserId = dto.UserId,
                User = user,
                ClientId = dto.ClientId,
                Client = client,
                CompanyId = dto.CompanyId,
                Company = company,
                Items = new List<BudgetItem>()
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                if (product == null)
                {
                    return NotFound($"Produto com ID {item.ProductId} não encontrado.");
                }
                var unitPrice = _pricingService.Calculate(product.BasePrice, client.State);
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

        //mover depois pra outra controller e filtras os dados enviados pro front
        [HttpGet("reference-data")]
        public async Task<IActionResult> GetReferenceData()
        {
            var users = await _appDbContext.UsersDB.ToListAsync();
            var companies = await _appDbContext.CompaniesDB.ToListAsync();
            var clients = await _appDbContext.ClientsDB.ToListAsync();
            var products = await _appDbContext.ProductsDB.ToListAsync();

            var response = new
            {
                users,
                companies,
                clients,
                products
            };

            return Ok(response);
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
