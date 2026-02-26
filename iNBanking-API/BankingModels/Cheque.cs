namespace BakingAPI.BankingModels
{
    public class Cheque
    {
        public int ChequeId { get; set; }
        public int AccountId { get; set; }
        public string ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DepositDate { get; set; }
        public string Status { get; set; } // "Not received", "Sent for Clearance", "Cleared", "Bounced"
        public DateTime? ClearanceDate { get; set; }

        public Account Account { get; set; }
    }
}
