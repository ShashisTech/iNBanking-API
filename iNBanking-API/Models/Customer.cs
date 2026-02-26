using System;
using System.Collections.Generic;

namespace BakingAPI.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime RegistrationDate { get; set; }

    public bool IsApproved { get; set; }

    public bool IsLocked { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
