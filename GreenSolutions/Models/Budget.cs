namespace GreenSolutions.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime CreatedAt { get; private set; }
        public List<BudgetItem> Items { get; set; } = new List<BudgetItem>();
        public decimal TotalPrice { get; set; }

        public Budget()
        {
            CreatedAt = DateTime.Now;
        }
        public Budget(int userId, User user, int clientId, Client client, List<BudgetItem> items)
        {
            UserId = userId;
            User = user;
            ClientId = clientId;
            Client = client;
            Items = items;
            CreatedAt = DateTime.Now;
        }
    }
}
