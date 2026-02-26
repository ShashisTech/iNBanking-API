using BakingAPI.BankingModels;
using BakingAPI.Dto;
using BakingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountManagementController : ControllerBase
    {
        private readonly BankingAppContext _context;

        public AccountManagementController(BankingAppContext context)
        {
            _context = context;
        }

        [HttpGet("/api/accounts/{customerId}")]
        public async Task<IActionResult> GetAccountsByCustomer(int customerId)
        {
            var accounts = await _context.Accounts
                .Where(a => a.CustomerId == customerId)
                .Select(a => new
                {
                    accountId = a.AccountId,
                    accountType = a.AccountType,
                    balance = a.Balance,
                    isActive = a.IsActive,
                    openDate = a.OpenDate
                })
                .ToListAsync();

            if (!accounts.Any())
                return NotFound(new { message = "No accounts found for this customer." });

            return Ok(accounts);
        }

        [HttpGet("/api/accounts/{accountId}/mini-statement")]
        public async Task<IActionResult> GetMiniStatement(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return NotFound(new { message = "Account not found." });

            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.TransactionId)
                .Take(5)
                .Select(t => new
                {
                    transactionId = t.TransactionId,
                    amount = t.Amount,
                    description = t.Description,
                    transactionType = t.TransactionType,
                    transactionDate = t.TransactionDate
                })
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("/api/accounts/{accountId}/statement")]
        public async Task<IActionResult> GetDetailedStatement(int accountId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
                return NotFound(new { message = "Account not found." });

            if (from > to)
                return BadRequest(new { message = "'from' date must be earlier than or equal to 'to' date." });

            var transactions = await _context.Transactions
                .Where(t => t.AccountId == accountId && t.TransactionDate.Date >= from.Date && t.TransactionDate.Date <= to.Date)
                .OrderBy(t => t.TransactionDate)
                .Select(t => new
                {
                    transactionId = t.TransactionId,
                    amount = t.Amount,
                    description = t.Description,
                    transactionType = t.TransactionType,
                    transactionDate = t.TransactionDate
                })
                .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("/api/accounts/{accountId}/balance")]
        public async Task<IActionResult> GetAccountBalance(int accountId)
        {
            var account = await _context.Accounts
                .Where(a => a.AccountId == accountId)
                .Select(a => new
                {
                    accountId = a.AccountId,
                    balance = a.Balance
                })
                .FirstOrDefaultAsync();

            if (account == null)
                return NotFound(new { message = "Account not found." });

            return Ok(account);
        }


    }
}
