namespace BakingAPI.Dto
{
    public class MoneyTransferDto
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
