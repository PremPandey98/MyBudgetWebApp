namespace WebApplication1.Models
{
    public class BudgetUsage
    {
        public int Id { get; set; }

        public decimal AmountUsed { get; set; }

        public DateTime DateUsed { get; set; }

        public string? UsageDescription { get; set; }

        // New fields for user/group tracking
        public int? UserId { get; set; }
        public BudgetMobApp.Models.User? User { get; set; }
        public int? GroupId { get; set; }
        public BudgetMobApp.Models.Group? Group { get; set; }
    }
}
