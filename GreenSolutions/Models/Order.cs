using GreenSolutions.Enums;

namespace GreenSolutions.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public int? ResponsibleUserId { get; set; }
        public User? ResponsibleUser { get; set; }

        public string Description { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }

        public ShippingType ShippingType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        public List<OrderItem> Items { get; set; } = new();
        public List<Budget> Budgets { get; set; } = new();

        public Order() { }

        public Order(int clientId, Client client, int companyId, Company company, int? responsibleUserId, User? responsibleUser,
            string description, List<OrderItem> items, DateTime? dueDate = null)
        {
            CreatedAt = DateTime.UtcNow;
            ClientId = clientId;
            Client = client;
            CompanyId = companyId;
            Company = company;
            ResponsibleUserId = responsibleUserId;
            ResponsibleUser = responsibleUser;
            Description = description;
            Status = OrderStatus.Open;
            ShippingType = string.Equals(client.State?.Trim(), "SP", StringComparison.OrdinalIgnoreCase)
                ? ShippingType.Driver
                : ShippingType.PostalService;
            DueDate = dueDate;
            Items = items;
        }
    }
}
