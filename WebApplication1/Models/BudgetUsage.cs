namespace WebApplication1.Models
{
    using System.ComponentModel.DataAnnotations;
    public class BudgetUsage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal AmountUsed { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime DateUsed { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string? UsageDescription { get; set; }

        [Required(ErrorMessage = "Spend By is required")]
        public string? SpendBy { get; set; }

        // New fields for user/group tracking
        public int? UserId { get; set; }
        public BudgetMobApp.Models.User? User { get; set; }
        public int? GroupId { get; set; }
        public BudgetMobApp.Models.Group? Group { get; set; }
    }
}
