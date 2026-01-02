namespace Expense.Logger.Business.Models.Transaction;

public class TransactionQuery : PageInfoQuery
{
    public DateTime? From { get; set; }

    public DateTime? To { get; set; }
}
