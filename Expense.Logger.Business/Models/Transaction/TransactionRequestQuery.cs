namespace Expense.Logger.Business.Models.Transactions;

public class TransactionRequestQuery : RequestQueryPageInfo
{
    public DateTime? From { get; set; }

    public DateTime? To { get; set; }
}
