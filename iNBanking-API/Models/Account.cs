using System;
using System.Collections.Generic;

namespace BakingAPI.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int CustomerId { get; set; }

    public string AccountType { get; set; } = null!;

    public decimal Balance { get; set; }

    public DateTime OpenDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BillPayment> BillPayments { get; set; } = new List<BillPayment>();

    public virtual ICollection<Cheque> Cheques { get; set; } = new List<Cheque>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<MoneyTransfer> MoneyTransferFromAccounts { get; set; } = new List<MoneyTransfer>();

    public virtual ICollection<MoneyTransfer> MoneyTransferToAccounts { get; set; } = new List<MoneyTransfer>();

    public virtual ICollection<Overdraft> Overdrafts { get; set; } = new List<Overdraft>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
