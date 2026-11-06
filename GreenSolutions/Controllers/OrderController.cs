using GreenSolutions.DTOs.BudgetDTOs;
using GreenSolutions.DTOs.OrderDTOs;
using GreenSolutions.Enums;
using GreenSolutions.Models;
using GreenSolutions.Persistence;
using GreenSolutions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenSolutions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PricingService _pricingService;

        public OrderController(AppDbContext appDbContext, PricingService pricingService)
        {
            _appDbContext = appDbContext;
            _pricingService = pricingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllOrders()
        {
            var orders = await OrdersWithDetails()
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders.Select(o => FormatOrderResponse(o, _pricingService)));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(int id)
        {
            var order = await OrdersWithDetails()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound("Pedido não encontrado.");

            return Ok(FormatOrderResponse(order, _pricingService));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderDTO value)
        {
            if (value.Items.Count == 0 || value.Items.Any(i => i.RequestedQuantity <= 0))
                return BadRequest("O pedido precisa ter pelo menos um item com quantidade válida.");

            var duplicateProductIds = value.Items
                .GroupBy(i => i.ProductId)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            if (duplicateProductIds.Count > 0)
                return BadRequest($"Produtos repetidos no pedido: {string.Join(", ", duplicateProductIds)}.");

            var client = await _appDbContext.ClientsDB.FindAsync(value.ClientId);
            if (client == null)
                return NotFound("Cliente não encontrado.");

            var company = await _appDbContext.CompaniesDB.FindAsync(value.CompanyId);
            if (company == null)
                return NotFound("Empresa não encontrada.");

            User? user = null;
            if (value.ResponsibleUserId.HasValue)
            {
                user = await _appDbContext.UsersDB.FindAsync(value.ResponsibleUserId.Value);
                if (user == null)
                    return NotFound("Usuário responsável não encontrado.");
            }

            var productIds = value.Items.Select(i => i.ProductId).ToList();
            var products = await _appDbContext.ProductsDB
                .Where(product => productIds.Contains(product.Id))
                .ToDictionaryAsync(product => product.Id);

            var missingProductIds = productIds
                .Where(productId => !products.ContainsKey(productId))
                .ToList();

            if (missingProductIds.Count > 0)
                return NotFound($"Produtos não encontrados: {string.Join(", ", missingProductIds)}.");

            var orderItems = value.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Product = products[item.ProductId],
                ProductIdSnapshot = products[item.ProductId].Id,
                ProductNameSnapshot = products[item.ProductId].Name,
                ProductBasePriceSnapshot = products[item.ProductId].BasePrice,
                RequestedQuantity = item.RequestedQuantity
            }).ToList();

            var order = new Order(
                value.ClientId,
                client,
                value.CompanyId,
                company,
                value.ResponsibleUserId,
                user,
                value.Description,
                orderItems,
                value.DueDate);

            _appDbContext.OrdersDB.Add(order);
            await _appDbContext.SaveChangesAsync();

            var createdOrder = await OrdersWithDetails()
                .FirstAsync(o => o.Id == order.Id);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = createdOrder.Id },
                FormatOrderResponse(createdOrder, _pricingService));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _appDbContext.OrdersDB.FindAsync(id);
            if (order == null)
                return NotFound("Pedido não encontrado.");

            _appDbContext.OrdersDB.Remove(order);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        private IQueryable<Order> OrdersWithDetails()
        {
            return _appDbContext.OrdersDB
                .AsNoTracking()
                .AsSplitQuery()
                .Include(o => o.Client)
                .Include(o => o.Company)
                .Include(o => o.ResponsibleUser)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.Items)
                    .ThenInclude(i => i.BudgetItems)
                        .ThenInclude(i => i.Budget)
                .Include(o => o.Budgets)
                    .ThenInclude(b => b.User)
                .Include(o => o.Budgets)
                    .ThenInclude(b => b.Client)
                .Include(o => o.Budgets)
                    .ThenInclude(b => b.Company)
                .Include(o => o.Budgets)
                    .ThenInclude(b => b.Items)
                        .ThenInclude(i => i.Product);
        }

        private static OrderResponseDTO FormatOrderResponse(Order order, PricingService pricingService)
        {
            return new OrderResponseDTO
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                DueDate = order.DueDate,
                Status = order.Status,
                ShippingType = order.ShippingType,
                TotalPrice = order.Items.Sum(item =>
                    pricingService.Calculate(
                        item.ProductBasePriceSnapshot, order.Client.State) * item.RequestedQuantity),
                BudgetedPrice = order.Budgets.Where(budget => IsAllocatingBudgetStatus(budget.Status)).Sum(budget => budget.TotalPrice),
                ClientId = order.ClientId,
                ClientName = order.Client.Name,
                CompanyId = order.CompanyId,
                CompanyName = order.Company.Name,
                ResponsibleUser = order.ResponsibleUser?.Name,
                Description = order.Description,
                OrderItems = order.Items.Select(item => new OrderItemResponseDTO
                {
                    OrderItemId = item.Id,
                    ProductId = item.ProductId ?? item.ProductIdSnapshot,
                    ProductName = item.Product?.Name ?? item.ProductNameSnapshot,
                    RequestedQuantity = item.RequestedQuantity,
                    BudgetedQuantity = item.BudgetItems
                        .Where(budgetItem => IsAllocatingBudgetStatus(budgetItem.Budget.Status))
                        .Sum(budgetItem => budgetItem.Quantity)
                }).ToList(),
                Budgets = order.Budgets.Select(budget => new BudgetResponseDTO
                {
                    Id = budget.Id,
                    OrderId = budget.OrderId,
                    CreatedAt = budget.CreatedAt,
                    Status = budget.Status,
                    TotalPrice = budget.TotalPrice,
                    ClientName = budget.Client?.Name ?? budget.ClientNameSnapshot,
                    CompanyName = budget.Company?.Name ?? budget.CompanyNameSnapshot,
                    UserName = budget.User?.Name ?? budget.UserNameSnapshot,
                    Items = budget.Items.Select(item => new BudgetItemResponseDTO
                    {
                        ProductId = item.ProductId ?? item.ProductIdSnapshot,
                        OrderItemId = item.OrderItemId,
                        ProductName = item.Product?.Name ?? item.ProductNameSnapshot,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList()
                }).ToList()
            };
        }

        private static bool IsAllocatingBudgetStatus(BudgetStatus status)
        {
            return status is not BudgetStatus.Rejected
                and not BudgetStatus.Expired
                and not BudgetStatus.Cancelled;
        }
    }
}
