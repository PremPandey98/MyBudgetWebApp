using WebApplication1.Models;
using BudgetMobApp.Models;

namespace BudgetMobApp.Models
{
    public enum AccountType
    {
        Personal,
        Group
    }

    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }

        public AccountType AccountType { get; set; }
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
