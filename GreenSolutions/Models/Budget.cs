namespace GreenSolutions.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string UserNameSnapshot { get; set; } = string.Empty;
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
        public string ClientNameSnapshot { get; set; } = string.Empty;
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public string CompanyNameSnapshot { get; set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public List<BudgetItem> Items { get; set; } = new List<BudgetItem>();
        public decimal TotalPrice { get; set; }

        public Budget()
        {
            CreatedAt = DateTime.Now;
        }
        public Budget(int? userId, User? user, string userNameSnapshot, int? clientId, Client? client, string clientNameSnapshot, int? companyId, Company? company, string companyNameSnapshot, List<BudgetItem> items)
        {
            UserId = userId;
            User = user;
            UserNameSnapshot = userNameSnapshot;
            ClientId = clientId;
            Client = client;
            ClientNameSnapshot = clientNameSnapshot;
            CompanyId = companyId;
            Company = company;
            CompanyNameSnapshot = companyNameSnapshot;
            Items = items;
            CreatedAt = DateTime.Now;
        }
    }
}
