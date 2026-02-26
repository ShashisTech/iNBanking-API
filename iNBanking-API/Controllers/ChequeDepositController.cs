using BakingAPI.BankingModels;
using BakingAPI.Dto;
using BakingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChequeDepositController : ControllerBase
    {
        private readonly BankingAppContext _context;

        public ChequeDepositController(BankingAppContext context)
        {
            _context = context;
        }

        [HttpPost("/api/cheques/deposit")]
        public async Task<IActionResult> DepositCheque([FromBody] ChequeDepositDto dto)
        {
            if (dto.Amount <= 0)
                return BadRequest(new { message = "Cheque amount must be positive." });

            var account = await _context.Accounts.FindAsync(dto.AccountId);
            if (account == null || !account.IsActive)
                return NotFound(new { message = "Account not found or inactive." });

            // Optionally, check for duplicate cheque number for this account
            if (_context.Cheques.Any(c => c.AccountId == dto.AccountId && c.ChequeNumber == dto.ChequeNumber))
                return Conflict(new { message = "Cheque number already submitted for this account." });

            var cheque = new BakingAPI.Models.Cheque
            {
                AccountId = dto.AccountId,
                ChequeNumber = dto.ChequeNumber,
                Amount = dto.Amount,
                DepositDate = DateTime.UtcNow,
                Status = "Not received"
            };

            _context.Cheques.Add(cheque);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cheque deposit submitted successfully.",
                chequeId = cheque.ChequeId,
                status = cheque.Status
            });
        }

        [HttpGet("/api/cheques/status/{customerId}")]
        public async Task<IActionResult> GetChequeStatus(int customerId)
        {
            // Find all accounts for the customer
            var accountIds = await _context.Accounts
                .Where(a => a.CustomerId == customerId)
                .Select(a => a.AccountId)
                .ToListAsync();

            if (!accountIds.Any())
                return NotFound(new { message = "No accounts found for this customer." });

            // Find all cheques for these accounts
            var cheques = await _context.Cheques
                .Where(c => accountIds.Contains(c.AccountId))
                .Select(c => new
                {
                    chequeId = c.ChequeId,
                    accountId = c.AccountId,
                    chequeNumber = c.ChequeNumber,
                    amount = c.Amount,
                    depositDate = c.DepositDate,
                    status = c.Status,
                    clearanceDate = c.ClearanceDate
                })
                .ToListAsync();

            return Ok(cheques);
        }

        [HttpGet("/api/cheques/reconciliation")]
        public async Task<IActionResult> GetChequeReconciliation([FromQuery] DateTime date)
        {
            // Get all cheques received up to the specified date
            var cheques = await _context.Cheques
                .Where(c => c.DepositDate.Date <= date.Date)
                .ToListAsync();

            var totalReceived = cheques.Count;
            var cleared = cheques.Count(c => c.Status == "Cleared");
            var bounced = cheques.Count(c => c.Status == "Bounced");
            var notCleared = cheques.Count(c => c.Status != "Cleared" && c.Status != "Bounced");

            return Ok(new
            {
                reportDate = date.Date,
                totalReceived,
                cleared,
                bounced,
                notCleared,
                cheques = cheques.Select(c => new
                {
                    chequeId = c.ChequeId,
                    accountId = c.AccountId,
                    chequeNumber = c.ChequeNumber,
                    amount = c.Amount,
                    depositDate = c.DepositDate,
                    status = c.Status,
                    clearanceDate = c.ClearanceDate
                })
            });
        }


    }
}
