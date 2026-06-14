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
        public DbSet<Company> CompaniesDB { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Budget>()
                .Property(b => b.UserNameSnapshot)
                .HasColumnType("longtext")
                .IsRequired();

            modelBuilder.Entity<Budget>()
                .Property(b => b.ClientNameSnapshot)
                .HasColumnType("longtext")
                .IsRequired();

            modelBuilder.Entity<Budget>()
                .Property(b => b.CompanyNameSnapshot)
                .HasColumnType("longtext")
                .IsRequired();

            modelBuilder.Entity<BudgetItem>()
                .Property(i => i.ProductNameSnapshot)
                .HasColumnType("longtext")
                .IsRequired();

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Client)
                .WithMany()
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Company)
                .WithMany()
                .HasForeignKey(b => b.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BudgetItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
