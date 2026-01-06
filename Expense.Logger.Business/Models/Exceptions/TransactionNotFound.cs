namespace Expense.Logger.Business.Models.Exceptions;

public class TransactionNotFound(long transactionId) : BusinessException($"Transaction with id {transactionId} not found.")
{
    public long TransactionId { get; set; } = transactionId;
}
