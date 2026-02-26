using BakingAPI.BankingModels;
using BakingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakingAPI.Dto;

namespace BakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationSecurityController : ControllerBase
    {
        private readonly BankingAppContext _context;

        public AuthenticationSecurityController(BankingAppContext context)
        {
            _context = context;
        }

        [HttpPost("/api/auth/login")]
        public async Task<IActionResult> Login([FromBody] CustomerLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Email and password are required." });

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (customer == null)
                return Unauthorized(new { message = "Invalid email or password." });

            // Hash the input password for comparison
            var inputHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto.Password));

            if (customer.PasswordHash != inputHash)
                return Unauthorized(new { message = "Invalid email or password." });

            if (customer.IsLocked)
                return Unauthorized(new { message = "Account is locked." });

            if (!customer.IsApproved)
                return Unauthorized(new { message = "Account is not approved." });

            // Login successful
            return Ok(new
            {
                customerId = customer.CustomerId,
                fullName = customer.FullName,
                email = customer.Email,
                message = "Login successful."
            });
        }

        [HttpPost("/api/auth/logout")]
        public IActionResult Logout()
        {
            // For stateless APIs (JWT or token-based), logout is handled on the client by removing the token.
            // If using server-side sessions, you could clear the session here.
            // Example for session-based logout:
            // HttpContext.Session.Clear();

            return Ok(new { message = "Logout successful. Please remove your authentication token on the client." });
        }

        [HttpPost("/api/auth/unlock")]
        public async Task<IActionResult> UnlockAccount([FromBody] UnlockAccountDto dto)
        {
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                return NotFound(new { message = "Customer not found." });

            if (!customer.IsLocked)
                return BadRequest(new { message = "Account is already unlocked." });

            customer.IsLocked = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Account unlocked successfully.", customerId = customer.CustomerId });
        }




    }
}
