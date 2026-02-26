namespace BakingAPI.BankingModels
{
    public class Overdraft
    {
        public int OverdraftId { get; set; }
        public int AccountId { get; set; }
        public decimal OverdraftAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Account Account { get; set; }
    }
}
