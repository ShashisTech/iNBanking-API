using System;
using System.Collections.Generic;

namespace BakingAPI.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Description { get; set; }

    public int? RelatedAccountId { get; set; }

    public virtual Account Account { get; set; } = null!;
}
