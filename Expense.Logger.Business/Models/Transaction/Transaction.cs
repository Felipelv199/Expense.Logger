namespace Expense.Logger.Business.Models.Transaction;

public class Transaction
{
    public long Id { get; set; }

    public string Name { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public TransactionCategory Category { get; set; }

    public TransactionType Type { get; set; }
}

public class TransactionCategory
{
    public long Id { get; set; }

    public string Name { get; set; }
}