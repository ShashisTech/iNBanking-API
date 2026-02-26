namespace BakingAPI.Dto
{
    public class CustomerRegistrationDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string AccountType { get; set; }
    }

    public class CustomerLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UnlockAccountDto
    {
        public int CustomerId { get; set; }
    }

}
