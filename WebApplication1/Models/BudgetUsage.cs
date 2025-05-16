namespace WebApplication1.Models
{
    public class BudgetUsage
    {
        public int Id { get; set; }

        public decimal AmountUsed { get; set; }

        public DateTime DateUsed { get; set; }

        public string UsageDescription { get; set; }
    }
}
