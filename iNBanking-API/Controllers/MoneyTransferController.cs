using BakingAPI.BankingModels;
using BakingAPI.Dto;
using BakingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoneyTransferController : ControllerBase
    {
        private readonly BankingAppContext _context;

        public MoneyTransferController(BankingAppContext context)
        {
            _context = context;
        }

        [HttpPost("/api/transfer")]
        public async Task<IActionResult> Transfer([FromBody] MoneyTransferDto dto)
        {
            if (dto.FromAccountId == dto.ToAccountId)
                return BadRequest(new { message = "Source and destination accounts must be different." });

            if (dto.Amount <= 0)
                return BadRequest(new { message = "Transfer amount must be positive." });

            var fromAccount = await _context.Accounts.FindAsync(dto.FromAccountId);
            var toAccount = await _context.Accounts.FindAsync(dto.ToAccountId);

            if (fromAccount == null || toAccount == null)
                return NotFound(new { message = "One or both accounts not found." });

            if (!fromAccount.IsActive || !toAccount.IsActive)
                return BadRequest(new { message = "Both accounts must be active." });

            if (fromAccount.Balance < dto.Amount)
                return BadRequest(new { message = "Insufficient balance in source account." });

            // Update balances
            fromAccount.Balance -= dto.Amount;
            toAccount.Balance += dto.Amount;

            // Log the transfer
            var transfer = new BakingAPI.Models.MoneyTransfer
            {
                FromAccountId = fromAccount.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = dto.Amount,
                TransferDate = DateTime.UtcNow,
                Status = "Completed"
            };
            _context.MoneyTransfers.Add(transfer);

            // Optionally, add transaction records
            _context.Transactions.Add(new BakingAPI.Models.Transaction
            {
                AccountId = fromAccount.AccountId,
                TransactionType = "Transfer Out",
                Amount = -dto.Amount,
                TransactionDate = DateTime.UtcNow,
                Description = $"Transfer to Account {toAccount.AccountId}",
                RelatedAccountId = toAccount.AccountId
            });
            _context.Transactions.Add(new BakingAPI.Models.Transaction
            {
                AccountId = toAccount.AccountId,
                TransactionType = "Transfer In",
                Amount = dto.Amount,
                TransactionDate = DateTime.UtcNow,
                Description = $"Transfer from Account {fromAccount.AccountId}",
                RelatedAccountId = fromAccount.AccountId
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Transfer successful.",
                transferId = transfer.TransferId,
                fromAccountId = fromAccount.AccountId,
                toAccountId = toAccount.AccountId,
                amount = dto.Amount
            });
        }

        [HttpGet("/api/transfer/history/{customerId}")]
        public async Task<IActionResult> GetMoneyTransferHistory(int customerId)
        {
            // Get all accounts for the customer
            var accountIds = await _context.Accounts
                .Where(a => a.CustomerId == customerId)
                .Select(a => a.AccountId)
                .ToListAsync();

            if (!accountIds.Any())
                return NotFound(new { message = "No accounts found for this customer." });

            // Get all money transfers where the customer is sender or receiver
            var transfers = await _context.MoneyTransfers
                .Where(mt => accountIds.Contains(mt.FromAccountId) || accountIds.Contains(mt.ToAccountId))
                .OrderByDescending(mt => mt.TransferDate)
                .Select(mt => new
                {
                    transferId = mt.TransferId,
                    fromAccountId = mt.FromAccountId,
                    toAccountId = mt.ToAccountId,
                    amount = mt.Amount,
                    transferDate = mt.TransferDate,
                    status = mt.Status
                })
                .ToListAsync();

            return Ok(transfers);
        }


    }
}
