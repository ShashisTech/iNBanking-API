using System;
using System.Collections.Generic;

namespace BakingAPI.Models;

public partial class Cheque
{
    public int ChequeId { get; set; }

    public int AccountId { get; set; }

    public string ChequeNumber { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime DepositDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ClearanceDate { get; set; }

    public virtual Account Account { get; set; } = null!;
}
