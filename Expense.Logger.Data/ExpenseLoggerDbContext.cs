using Expense.Logger.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense.Logger.Data;

public class ExpenseLoggerDbContext(DbContextOptions<ExpenseLoggerDbContext> options) : DbContext(options)
{
     public DbSet<BankAccounts> BankAccounts { get; set; }

    public DbSet<Categories> Categories { get; set; }

    public DbSet<Transactions> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories>().HasMany(category => category.Transactions)
            .WithOne(transaction => transaction.Category)
            .HasForeignKey(transaction => transaction.CategoryId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        modelBuilder.Entity<BankAccounts>().HasMany(bankAccount => bankAccount.Transactions)
            .WithOne(transaction => transaction.BankAccounts)
            .HasForeignKey(transaction => transaction.BankAccountId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
