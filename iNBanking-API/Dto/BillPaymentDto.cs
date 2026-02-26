namespace BakingAPI.Dto
{
    public class BillPaymentDto
    {
        public int AccountId { get; set; }
        public string BillerName { get; set; }
        public decimal Amount { get; set; }
    }
}
