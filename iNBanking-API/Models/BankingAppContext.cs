using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BakingAPI.Models;

public partial class BankingAppContext : DbContext
{
    public BankingAppContext()
    {
    }

    public BankingAppContext(DbContextOptions<BankingAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<BillPayment> BillPayments { get; set; }

    public virtual DbSet<Cheque> Cheques { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<MoneyTransfer> MoneyTransfers { get; set; }

    public virtual DbSet<Overdraft> Overdrafts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A6004EC1DD");

            entity.Property(e => e.AccountType).HasMaxLength(20);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OpenDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__Custom__2B3F6F97");
        });

        modelBuilder.Entity<BillPayment>(entity =>
        {
            entity.HasKey(e => e.BillPaymentId).HasName("PK__BillPaym__9BD090C7687DD938");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BillerName).HasMaxLength(100);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.ScheduledDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.BillPayments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillPayme__Accou__33D4B598");
        });

        modelBuilder.Entity<Cheque>(entity =>
        {
            entity.HasKey(e => e.ChequeId).HasName("PK__Cheques__B816D9F0DDF98A24");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChequeNumber).HasMaxLength(50);
            entity.Property(e => e.ClearanceDate).HasColumnType("datetime");
            entity.Property(e => e.DepositDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.Cheques)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cheques__Account__30F848ED");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.ConfigKey).HasName("PK__Configur__4A3067852C98DCFD");

            entity.Property(e => e.ConfigKey).HasMaxLength(50);
            entity.Property(e => e.ConfigValue).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D800C27FAE");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D10534CEAA4ECE").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MoneyTransfer>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__MoneyTra__954900910607A8DC");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TransferDate).HasColumnType("datetime");

            entity.HasOne(d => d.FromAccount).WithMany(p => p.MoneyTransferFromAccounts)
                .HasForeignKey(d => d.FromAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MoneyTran__FromA__36B12243");

            entity.HasOne(d => d.ToAccount).WithMany(p => p.MoneyTransferToAccounts)
                .HasForeignKey(d => d.ToAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MoneyTran__ToAcc__37A5467C");
        });

        modelBuilder.Entity<Overdraft>(entity =>
        {
            entity.HasKey(e => e.OverdraftId).HasName("PK__Overdraf__861BD84DE6E4E25D");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.OverdraftAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Overdrafts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Overdraft__Accou__3A81B327");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B778C3242");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Accou__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
