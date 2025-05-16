namespace WebApplication1.Models
{
    public class BudgetDeposit
    {
        public int Id { get; set; }

        public decimal DepositAmount { get; set; }

        public string DepositedBy { get; set; }

        public DateTime DepositDate { get; set; }

    }
}
