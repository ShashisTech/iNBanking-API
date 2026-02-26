namespace BakingAPI.BankingModels
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string TransactionType { get; set; } // "Deposit", "Withdrawal", etc.
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public int? RelatedAccountId { get; set; } // For transfers

        public Account Account { get; set; }
    }
}
