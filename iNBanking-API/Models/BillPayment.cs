using System;
using System.Collections.Generic;

namespace BakingAPI.Models;

public partial class BillPayment
{
    public int BillPaymentId { get; set; }

    public int AccountId { get; set; }

    public string BillerName { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime ScheduledDate { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;
}
