using GreenSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenSolutions.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> UsersDB { get; set;  }

        public DbSet<Client> ClientDB { get; set; }
    }
}
