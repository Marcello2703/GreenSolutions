namespace GreenSolutions.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int? ProductId { get; set; }
        public int ProductIdSnapshot { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public decimal ProductBasePriceSnapshot { get; set; }
        public Product? Product { get; set; }

        public int RequestedQuantity { get; set; }

        public List<BudgetItem> BudgetItems { get; set; } = new();
    }
}
