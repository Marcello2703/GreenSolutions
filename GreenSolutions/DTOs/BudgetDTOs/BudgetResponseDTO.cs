using GreenSolutions.Enums;

namespace GreenSolutions.DTOs.BudgetDTOs
{
    public class BudgetResponseDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public BudgetStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<BudgetItemResponseDTO> Items { get; set; } = new();
    }
}
