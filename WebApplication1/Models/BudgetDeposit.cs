namespace WebApplication1.Models
{
    public class BudgetDeposit
    {
        public int Id { get; set; }

        public decimal DepositAmount { get; set; }

        public string DepositedBy { get; set; }

        public DateTime DepositDate { get; set; }

        // New fields for user/group tracking
        public int? UserId { get; set; }
        public BudgetMobApp.Models.User? User { get; set; }
        public int? GroupId { get; set; }
        public BudgetMobApp.Models.Group? Group { get; set; }
    }
}
