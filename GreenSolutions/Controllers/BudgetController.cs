using GreenSolutions.DTOs.BudgetDTOs;
using GreenSolutions.Enums;
using GreenSolutions.Models;
using GreenSolutions.Persistence;
using GreenSolutions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBudgetById(int id)
        {
            var budget = await BudgetsWithDetails()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (budget == null)
                return NotFound("Orçamento não encontrado.");

            return Ok(FormatBudgetResponse(budget));
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgets()
        {
            var budgets = await BudgetsWithDetails()
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Ok(budgets.Select(FormatBudgetResponse));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDTO dto)
        {
            if (dto.Items.Count == 0 || dto.Items.Any(i => i.Quantity <= 0))
                return BadRequest("O orçamento precisa ter pelo menos um item válido.");

            var user = await _appDbContext.UsersDB.FindAsync(dto.UserId);
            if (user == null)
                return NotFound("Usuário não encontrado.");

            Order? order = null;
            Client? client;
            Company? company;
            Dictionary<int, OrderItem>? orderItems = null;
            Dictionary<int, int> allocatedQuantities = new();
            Dictionary<int, Product>? standaloneProducts = null;

            if (dto.OrderId.HasValue)
            {
                order = await _appDbContext.OrdersDB
                    .Include(o => o.Client)
                    .Include(o => o.Company)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(o => o.Id == dto.OrderId.Value);

                if (order == null)
                    return NotFound("Pedido não encontrado.");

                if (dto.Items.Any(item => !item.OrderItemId.HasValue))
                    return BadRequest("Todo item de um orçamento vinculado deve informar o OrderItemId.");

                var duplicateOrderItemIds = dto.Items
                    .GroupBy(item => item.OrderItemId!.Value)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key)
                    .ToList();

                if (duplicateOrderItemIds.Count > 0)
                    return BadRequest($"Itens do pedido repetidos no orçamento: {string.Join(", ", duplicateOrderItemIds)}.");

                orderItems = order.Items.ToDictionary(item => item.Id);
                var requestedOrderItemIds = dto.Items
                    .Select(item => item.OrderItemId!.Value)
                    .ToList();

                var invalidOrderItemIds = requestedOrderItemIds
                    .Where(orderItemId => !orderItems.ContainsKey(orderItemId))
                    .ToList();

                if (invalidOrderItemIds.Count > 0)
                    return BadRequest($"Itens não pertencem ao pedido informado: {string.Join(", ", invalidOrderItemIds)}.");

                var allOrderItemsIds = order.Items.Select(item => item.Id).ToList();

                allocatedQuantities = await _appDbContext.BudgetItemsDB
                    .Where(budgetItem =>
                        budgetItem.OrderItemId.HasValue &&
                        allOrderItemsIds.Contains(budgetItem.OrderItemId.Value) &&
                        budgetItem.Budget.Status != BudgetStatus.Rejected &&
                        budgetItem.Budget.Status != BudgetStatus.Expired &&
                        budgetItem.Budget.Status != BudgetStatus.Cancelled)
                    .GroupBy(budgetItem => budgetItem.OrderItemId!.Value)
                    .ToDictionaryAsync(
                        group => group.Key, 
                        group => group.Sum(budgetItem => budgetItem.Quantity));

                foreach (var item in dto.Items)
                {
                    var orderItem = orderItems[item.OrderItemId!.Value];
                    var alreadyAllocated = allocatedQuantities.GetValueOrDefault(orderItem.Id);

                    if (alreadyAllocated + item.Quantity > orderItem.RequestedQuantity)
                    {
                        return BadRequest(
                            $"A quantidade do item #{orderItem.Id} excede a quantidade solicitada. " +
                            $"Solicitado: {orderItem.RequestedQuantity}; já orçado: {alreadyAllocated}; novo: {item.Quantity}.");
                    }
                }

                if (dto.ClientId.HasValue && dto.ClientId.Value != order.ClientId)
                    return BadRequest("O cliente informado não corresponde ao cliente do pedido.");

                if (dto.CompanyId.HasValue && dto.CompanyId.Value != order.CompanyId)
                    return BadRequest("A empresa informada não corresponde à empresa do pedido.");

                client = order.Client;
                company = order.Company;
            }
            else
            {
                if (!dto.ClientId.HasValue || !dto.CompanyId.HasValue)
                    return BadRequest("Orçamentos sem pedido devem informar ClientId e CompanyId.");

                if (dto.Items.Any(item => !item.ProductId.HasValue))
                    return BadRequest("Orçamentos sem pedido devem informar o ProductId de todos os itens.");

                client = await _appDbContext.ClientsDB.FindAsync(dto.ClientId.Value);
                if (client == null)
                    return NotFound("Cliente não encontrado.");

                company = await _appDbContext.CompaniesDB.FindAsync(dto.CompanyId.Value);
                if (company == null)
                    return NotFound("Empresa não encontrada.");

                var duplicateProductIds = dto.Items
                    .GroupBy(item => item.ProductId!.Value)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key)
                    .ToList();

                if (duplicateProductIds.Count > 0)
                    return BadRequest($"Produtos repetidos no orçamento: {string.Join(", ", duplicateProductIds)}.");

                var productIds = dto.Items.Select(item => item.ProductId!.Value).ToList();
                standaloneProducts = await _appDbContext.ProductsDB
                    .Where(product => productIds.Contains(product.Id))
                    .ToDictionaryAsync(product => product.Id);

                var missingProductIds = productIds
                    .Where(productId => !standaloneProducts.ContainsKey(productId))
                    .ToList();

                if (missingProductIds.Count > 0)
                    return NotFound($"Produtos não encontrados: {string.Join(", ", missingProductIds)}.");
            }

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
                OrderId = order?.Id,
                Order = order,
                Status = BudgetStatus.Draft,
                Items = new List<BudgetItem>()
            };

            foreach (var item in dto.Items)
            {
                var orderItem = item.OrderItemId.HasValue
                    ? orderItems![item.OrderItemId.Value]
                    : null;
                var product = orderItem?.Product;
                var productId = orderItem?.ProductId;
                var productIdSnapshot = orderItem?.ProductIdSnapshot ?? 0;
                var productNameSnapshot = orderItem?.ProductNameSnapshot ?? string.Empty;
                var productBasePrice = orderItem?.ProductBasePriceSnapshot ?? 0;

                if (orderItem == null)
                {
                    product = standaloneProducts![item.ProductId!.Value];
                    productId = product.Id;
                    productIdSnapshot = product.Id;
                    productNameSnapshot = product.Name;
                    productBasePrice = product.BasePrice;
                }
                var unitPrice = _pricingService.Calculate(productBasePrice, client.State);
                var totalPrice = unitPrice * item.Quantity;

                budget.Items.Add(new BudgetItem
                {
                    ProductId = productId,
                    Product = product,
                    ProductIdSnapshot = productIdSnapshot,
                    ProductNameSnapshot = productNameSnapshot,
                    OrderItemId = orderItem?.Id,
                    OrderItem = orderItem,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice
                });

                budget.TotalPrice += totalPrice;
            }

            if (order != null)
                UpdateOrderCoverageStatus(order, allocatedQuantities, dto.Items);

            _appDbContext.BudgetsDB.Add(budget);
            await _appDbContext.SaveChangesAsync();

            var createdBudget = await BudgetsWithDetails()
                .FirstAsync(b => b.Id == budget.Id);

            return CreatedAtAction(
                nameof(GetBudgetById),
                new { id = createdBudget.Id },
                FormatBudgetResponse(createdBudget));
        }

        [HttpGet("reference-data")]
        public async Task<IActionResult> GetReferenceData()
        {
            var users = await _appDbContext.UsersDB.AsNoTracking().ToListAsync();
            var companies = await _appDbContext.CompaniesDB.AsNoTracking().ToListAsync();
            var clients = await _appDbContext.ClientsDB.AsNoTracking().ToListAsync();
            var products = await _appDbContext.ProductsDB.AsNoTracking().ToListAsync();

            return Ok(new { users, companies, clients, products });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var budget = await _appDbContext.BudgetsDB.FindAsync(id);
            if (budget == null)
                return NotFound("Orçamento não encontrado na base.");

            var orderId = budget.OrderId;
            _appDbContext.BudgetsDB.Remove(budget);
            await _appDbContext.SaveChangesAsync();

            if (orderId.HasValue)
                await RecalculateOrderCoverageStatus(orderId.Value);

            return NoContent();
        }

        private IQueryable<Budget> BudgetsWithDetails()
        {
            return _appDbContext.BudgetsDB
                .AsNoTracking()
                .Include(b => b.Client)
                .Include(b => b.Company)
                .Include(b => b.User)
                .Include(b => b.Items)
                    .ThenInclude(i => i.Product);
        }

        private async Task RecalculateOrderCoverageStatus(int orderId)
        {
            var order = await _appDbContext.OrdersDB
                .Include(o => o.Items)
                    .ThenInclude(i => i.BudgetItems)
                        .ThenInclude(i => i.Budget)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || !IsCoverageStatus(order.Status))
                return;

            var budgetedQuantity = order.Items.Sum(item => item.BudgetItems
                .Where(budgetItem => IsAllocatingBudgetStatus(budgetItem.Budget.Status))
                .Sum(budgetItem => budgetItem.Quantity));
            var requestedQuantity = order.Items.Sum(item => item.RequestedQuantity);

            order.Status = budgetedQuantity == 0
                ? OrderStatus.Open
                : budgetedQuantity >= requestedQuantity
                    ? OrderStatus.FullyBudgeted
                    : OrderStatus.PartiallyBudgeted;

            await _appDbContext.SaveChangesAsync();
        }

        private static void UpdateOrderCoverageStatus(
            Order order,
            IReadOnlyDictionary<int, int> allocatedQuantities,
            IEnumerable<BudgetItemDTO> newItems)
        {
            if (!IsCoverageStatus(order.Status))
                return;

            var newQuantities = newItems.ToDictionary(
                item => item.OrderItemId!.Value,
                item => item.Quantity);

            var fullyBudgeted = order.Items.All(item =>
                allocatedQuantities.GetValueOrDefault(item.Id) +
                newQuantities.GetValueOrDefault(item.Id) >= item.RequestedQuantity);

            order.Status = fullyBudgeted
                ? OrderStatus.FullyBudgeted
                : OrderStatus.PartiallyBudgeted;
        }

        private static bool IsCoverageStatus(OrderStatus status)
        {
            return status is OrderStatus.Open
                or OrderStatus.PartiallyBudgeted
                or OrderStatus.FullyBudgeted;
        }

        private static bool IsAllocatingBudgetStatus(BudgetStatus status)
        {
            return status is not BudgetStatus.Rejected
                and not BudgetStatus.Expired
                and not BudgetStatus.Cancelled;
        }

        private static BudgetResponseDTO FormatBudgetResponse(Budget budget)
        {
            return new BudgetResponseDTO
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
            };
        }
    }
}
