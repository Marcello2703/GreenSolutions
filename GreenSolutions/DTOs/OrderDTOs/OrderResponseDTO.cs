using GreenSolutions.DTOs.BudgetDTOs;
using GreenSolutions.Enums;

namespace GreenSolutions.DTOs.OrderDTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public OrderStatus Status { get; set; }
        public ShippingType ShippingType { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal BudgetedPrice { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? ResponsibleUser { get; set; }
        public string? Description { get; set; }
        public List<OrderItemResponseDTO> OrderItems { get; set; } = new();
        public List<BudgetResponseDTO> Budgets { get; set; } = new();
    }
}
