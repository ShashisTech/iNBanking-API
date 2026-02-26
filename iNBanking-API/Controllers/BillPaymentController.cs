using BakingAPI.BankingModels;
using BakingAPI.Dto;
using BakingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillPaymentController : ControllerBase
    {
        private readonly BankingAppContext _context;

        public BillPaymentController(BankingAppContext context)
        {
            _context = context;
        }

        [HttpPost("/api/bills/pay")]
        public async Task<IActionResult> PayBill([FromBody] BillPaymentDto dto)
        {
            if (dto.Amount <= 0)
                return BadRequest(new { message = "Bill amount must be positive." });

            var account = await _context.Accounts.FindAsync(dto.AccountId);
            if (account == null || !account.IsActive)
                return NotFound(new { message = "Account not found or inactive." });

            if (account.Balance < dto.Amount)
                return BadRequest(new { message = "Insufficient balance in account." });

            // Deduct the amount
            account.Balance -= dto.Amount;

            // Create bill payment record
            var billPayment = new BakingAPI.Models.BillPayment
            {
                AccountId = dto.AccountId,
                BillerName = dto.BillerName,
                Amount = dto.Amount,
                ScheduledDate = DateTime.UtcNow,
                PaymentDate = DateTime.UtcNow,
                Status = "Paid"
            };
            _context.BillPayments.Add(billPayment);

            // Optionally, add a transaction record
            _context.Transactions.Add(new BakingAPI.Models.Transaction
            {
                AccountId = dto.AccountId,
                TransactionType = "Bill Payment",
                Amount = -dto.Amount,
                TransactionDate = DateTime.UtcNow,
                Description = $"Bill payment to {dto.BillerName}"
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Bill paid successfully.",
                billPaymentId = billPayment.BillPaymentId,
                status = billPayment.Status,
                remainingBalance = account.Balance
            });
        }

        [HttpPost("/api/bills/schedule")]
        public async Task<IActionResult> ScheduleBill([FromBody] BillPaymentDto dto)
        {
            if (dto.Amount <= 0)
                return BadRequest(new { message = "Bill amount must be positive." });

            var account = await _context.Accounts.FindAsync(dto.AccountId);
            if (account == null || !account.IsActive)
                return NotFound(new { message = "Account not found or inactive." });

            // BillPaymentDto does not have ScheduledDate, so always use DateTime.UtcNow as scheduled date.
            var scheduledDate = DateTime.UtcNow;

            var billPayment = new BakingAPI.Models.BillPayment
            {
                AccountId = dto.AccountId,
                BillerName = dto.BillerName,
                Amount = dto.Amount,
                ScheduledDate = scheduledDate,
                PaymentDate = null,
                Status = "Scheduled"
            };
            _context.BillPayments.Add(billPayment);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Bill payment scheduled successfully.",
                billPaymentId = billPayment.BillPaymentId,
                status = billPayment.Status,
                scheduledDate = billPayment.ScheduledDate
            });
        }

        [HttpGet("/api/bills/history/{customerId}")]
        public async Task<IActionResult> GetBillPaymentHistory(int customerId)
        {
            // Get all accounts for the customer
            var accountIds = await _context.Accounts
                .Where(a => a.CustomerId == customerId)
                .Select(a => a.AccountId)
                .ToListAsync();

            if (!accountIds.Any())
                return NotFound(new { message = "No accounts found for this customer." });

            // Get all bill payments for these accounts
            var billPayments = await _context.BillPayments
                .Where(bp => accountIds.Contains(bp.AccountId))
                .OrderByDescending(bp => bp.PaymentDate ?? bp.ScheduledDate)
                .Select(bp => new
                {
                    billPaymentId = bp.BillPaymentId,
                    accountId = bp.AccountId,
                    billerName = bp.BillerName,
                    amount = bp.Amount,
                    scheduledDate = bp.ScheduledDate,
                    paymentDate = bp.PaymentDate,
                    status = bp.Status
                })
                .ToListAsync();

            return Ok(billPayments);
        }








    }
}
