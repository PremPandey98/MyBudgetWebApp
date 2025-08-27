namespace WebApplication1.Models
{
    using System.ComponentModel.DataAnnotations;
    public class BudgetDeposit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Deposit amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Deposit amount must be greater than 0")]
        public decimal DepositAmount { get; set; }

        [Required(ErrorMessage = "Deposited By is required")]
        [StringLength(50, ErrorMessage = "Deposited By cannot be longer than 50 characters.")]
        public string? DepositedBy { get; set; }

        [Required(ErrorMessage = "Deposit date is required")]
        public DateTime DepositDate { get; set; }

        // New fields for user/group tracking
        public int? UserId { get; set; }
        public BudgetMobApp.Models.User? User { get; set; }
        public int? GroupId { get; set; }
        public BudgetMobApp.Models.Group? Group { get; set; }
    }
}
