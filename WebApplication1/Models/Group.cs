using System.Collections.Generic;

namespace BudgetMobApp.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string? GroupName { get; set; }
        public string? GroupCode { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
