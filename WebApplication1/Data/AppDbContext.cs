using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using BudgetMobApp.Models;

namespace WebApplication1.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<BudgetDeposit> BudgetDeposits { get; set; }
        public DbSet<BudgetUsage> BudgetUsages { get; set; }
        public DbSet<BudgetMobApp.Models.User> Users { get; set; }
        public DbSet<BudgetMobApp.Models.Group> Groups { get; set; }
    }
}
