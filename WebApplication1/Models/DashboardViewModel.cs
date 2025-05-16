namespace WebApplication1.Models
{
    public class DashboardViewModel
    {
        public List<BudgetUsage> BudgetUsages { get; set; }
        public List<BudgetDeposit> BudgetDeposits { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalDeposits { get; set; }
        public decimal TotalUsages { get; set; }
        public decimal RemainingBalance => TotalDeposits - TotalUsages;

        // Helper property for display
        public string FormattedDateRange =>
            $"{StartDate:dd MMM yyyy} to {EndDate:dd MMM yyyy}";
    }
}
