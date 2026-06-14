namespace GreenSolutions.DTOs.BudgetDTOs
{
        public class BudgetResponseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string ClientName { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public List<BudgetItemResponseDTO> Items { get; set; } = new();
    }
}