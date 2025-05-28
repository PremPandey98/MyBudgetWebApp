using WebApplication1.Models;

namespace BudgetMobApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
        public ICollection<BudgetDeposit> BudgetDeposits { get; set; }
    }
}
