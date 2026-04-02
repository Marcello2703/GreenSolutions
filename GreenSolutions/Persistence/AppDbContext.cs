using GreenSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenSolutions.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> UsersDB { get; set;  }

        public DbSet<Client> ClientsDB { get; set; }

        public DbSet<Product> ProductsDB { get; set; }

        public DbSet<Budget> BudgetsDB { get; set; }
        public DbSet<BudgetItem> BudgetItemsDB { get; set; }
    }
}
