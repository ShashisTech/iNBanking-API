namespace BakingAPI.BankingModels
{
    public class BillPayment
    {
        public int BillPaymentId { get; set; }
        public int AccountId { get; set; }
        public string BillerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Status { get; set; } // "Scheduled", "Paid", "Failed"

        public Account Account { get; set; }
    }
}
