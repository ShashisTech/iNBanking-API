using System;
using System.Collections.Generic;

namespace BakingAPI.Models;

public partial class MoneyTransfer
{
    public int TransferId { get; set; }

    public int FromAccountId { get; set; }

    public int ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransferDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account FromAccount { get; set; } = null!;

    public virtual Account ToAccount { get; set; } = null!;
}
