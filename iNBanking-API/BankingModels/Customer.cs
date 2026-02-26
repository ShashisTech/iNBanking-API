namespace BakingAPI.BankingModels
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLocked { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
