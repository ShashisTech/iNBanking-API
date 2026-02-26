using BakingAPI.BankingModels;
using BakingAPI.Models;
using BakingAPI.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRegistrationController : ControllerBase
    {
        private readonly BankingAppContext _context;

        public CustomerRegistrationController(BankingAppContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Customers.Any(c => c.Email == dto.Email))
                return Conflict("Email already registered.");

            var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password));

            // Fix: Ensure the correct namespace/type is used for the Customer model
            var customer = new BakingAPI.Models.Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                RegistrationDate = DateTime.UtcNow,
                IsApproved = true,
                IsLocked = false,
                PasswordHash = passwordHash
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Optionally, create an account if AccountType is provided
            if (!string.IsNullOrEmpty(dto.AccountType))
            {
                var account = new BakingAPI.Models.Account
                {
                    CustomerId = customer.CustomerId,
                    AccountType = dto.AccountType,
                    Balance = 50000,
                    OpenDate = DateTime.UtcNow,
                    IsActive = true
                };
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(Register), new { customerId = customer.CustomerId }, customer);
        }

        [HttpGet("/api/customers/status/{customerId}")]
        public async Task<IActionResult> GetStatus(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
                return NotFound(new { message = "Customer not found." });

            return Ok(new
            {
                customerId = customer.CustomerId,
                fullName = customer.FullName,
                email = customer.Email,
                isApproved = customer.IsApproved,
                isLocked = customer.IsLocked,
                registrationDate = customer.RegistrationDate
            });
        }


    }
}



