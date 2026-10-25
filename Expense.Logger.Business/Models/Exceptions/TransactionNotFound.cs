namespace Expense.Logger.Business.Models.Exceptions;

public class TransactionNotFound(long id) : BusinessException($"Transaction with id {id} not found.")
{
    public long Id { get; set; } = id;
}
