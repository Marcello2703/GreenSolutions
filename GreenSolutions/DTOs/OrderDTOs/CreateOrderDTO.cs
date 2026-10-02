namespace GreenSolutions.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public int ClientId { get; set; }
        public int CompanyId { get; set; }
        public int? ResponsibleUserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }
}
