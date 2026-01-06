namespace Expense.Logger.Data.Models;

public class TransactionPageInfo
{
    public int Take { get; set; }

    public DateTime LastDate { get; set; }

    public long? LastTransactionId { get; set; }

    public DateTime? From {  get; set; }

    public DateTime? To { get; set; }
}
