using Microsoft.EntityFrameworkCore;

namespace Expense.Logger.Data.Models;

[PrimaryKey(nameof(TransactionId))]
public class Transactions : BaseDataModel
{
    public long TransactionId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public long? BankAccountId { get; set; } = null;

    public string Description { get; set; }

    public long? CategoryId { get; set; } = null;

    [Comment("0: Expense, 1: Income, 2: Transfer")]
    public int Type { get; set; } // 0: Expense, 1: Income, 2: Transfer

    public Categories Category { get; set; }

    public BankAccounts BankAccounts { get; set; }
}
