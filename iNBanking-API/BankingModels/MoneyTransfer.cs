namespace BakingAPI.BankingModels
{
    public class MoneyTransfer
    {
        public int TransferId { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public string Status { get; set; } // "Completed", "Failed"

        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }
    }
}
