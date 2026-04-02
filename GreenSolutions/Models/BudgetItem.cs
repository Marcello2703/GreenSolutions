using System.ComponentModel.DataAnnotations.Schema;

namespace GreenSolutions.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BudgetId { get; set; }
        public Budget Budget { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}