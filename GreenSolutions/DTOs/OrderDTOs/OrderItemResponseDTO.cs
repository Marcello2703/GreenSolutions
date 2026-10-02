namespace GreenSolutions.DTOs.OrderDTOs
{
    public class OrderItemResponseDTO
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int RequestedQuantity { get; set; }
        public int BudgetedQuantity { get; set; }
    }
}
