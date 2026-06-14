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
        [HttpGet("{id:int}")]
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

            var budgetResponse = FormatBudgetResponse(budget);

            return Ok(budgetResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgets()
        {
            var budgets = await _appDbContext.BudgetsDB.AsNoTracking()
                .Include(b => b.Client)
                .Include(b => b.Company)
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            var budgetsDTO = budgets.Select(b => FormatBudgetResponse(b)).ToList();

            return Ok(budgetsDTO);
        }

        // POST api/<BudgetController>
        [HttpPost]
        public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDTO dto)
        {
            if (dto == null)
                return BadRequest("Payload inválido.");
      
            if (dto.Items == null || dto.Items.Count == 0 || dto.Items.Any(i => i.Quantity <= 0))
                return BadRequest("O orçamento precisa ter pelo menos um item válido.");

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
            var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _appDbContext.ProductsDB.Where(p => productIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id);

            var missingProductIds = productIds
                .Where(id => !products.ContainsKey(id))
                .ToList();

            if (missingProductIds.Count > 0)
                return NotFound($"Produtos nao encontrados: {string.Join(", ", missingProductIds)}.");

            var budget = new Budget
            {
                UserId = user.Id,
                User = user,
                UserNameSnapshot = user.Name,
                ClientId = client.Id,
                Client = client,
                ClientNameSnapshot = client.Name,
                CompanyId = company.Id,
                Company = company,
                CompanyNameSnapshot = company.Name,
                Items = new List<BudgetItem>()
            };

            decimal total = 0;

            foreach (var item in dto.Items)
            {
                var product = products[item.ProductId];
                var unitPrice = _pricingService.Calculate(product.BasePrice, client.State);
                var totalPrice = unitPrice * item.Quantity;
                budget.Items.Add(new BudgetItem
                {
                    ProductId = item.ProductId,
                    ProductIdSnapshot = product.Id,
                    ProductNameSnapshot = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice
                });
                total += totalPrice;
            }

            budget.TotalPrice = total;

            _appDbContext.BudgetsDB.Add(budget);
            await _appDbContext.SaveChangesAsync();

            var createdBudget = await _appDbContext.BudgetsDB
                .AsNoTracking()
                .Include(b => b.Client)
                .Include(b => b.Company)
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product)
                .FirstAsync(b => b.Id == budget.Id);

            return CreatedAtAction(nameof(GetBudgetById), new { id = createdBudget.Id }, FormatBudgetResponse(createdBudget));
        }

        //mover depois pra outra controller e filtras os dados enviados pro front
        [HttpGet("reference-data")]
        public async Task<IActionResult> GetReferenceData()
        {
            var users = await _appDbContext.UsersDB.AsNoTracking().ToListAsync();
            var companies = await _appDbContext.CompaniesDB.AsNoTracking().ToListAsync();
            var clients = await _appDbContext.ClientsDB.AsNoTracking().ToListAsync();
            var products = await _appDbContext.ProductsDB.AsNoTracking().ToListAsync();

            var response = new
            {
                users,
                companies,
                clients,
                products
            };

            return Ok(response);
        }

        // DELETE api/<BudgetController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var budget = await _appDbContext.BudgetsDB.FindAsync(id);
            if (budget == null)
            {
                return NotFound("Orçamento não encontrado na base.");
            }

            _appDbContext.BudgetsDB.Remove(budget);
            await _appDbContext.SaveChangesAsync();

            return Ok($"Budget #{id} deletado.");
        }

        private static BudgetResponseDTO FormatBudgetResponse(Budget budget)
        {
            return new BudgetResponseDTO
            {
                Id = budget.Id,
                CreatedAt = budget.CreatedAt,
                TotalPrice = budget.TotalPrice,
                ClientName = budget.Client?.Name ?? budget.ClientNameSnapshot,
                CompanyName = budget.Company?.Name ?? budget.CompanyNameSnapshot,
                UserName = budget.User?.Name ?? budget.UserNameSnapshot,
                Items = budget.Items.Select(i => new BudgetItemResponseDTO
                {
                    ProductId = i.ProductId ?? i.ProductIdSnapshot,
                    ProductName = i.Product?.Name ?? i.ProductNameSnapshot,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
    }
}
