namespace BakingAPI.BankingModels
{
    public class Account
    {
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public string AccountType { get; set; } // "Savings" or "Current"
        public decimal Balance { get; set; }
        public DateTime OpenDate { get; set; }
        public bool IsActive { get; set; }

        public Customer Customer { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Cheque> Cheques { get; set; }
        public ICollection<BillPayment> BillPayments { get; set; }
        public ICollection<Overdraft> Overdrafts { get; set; }
    }
}
