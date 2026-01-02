namespace Expense.Logger.Business.Models.Exceptions;

public class TransactionCategoryNotFound(long categoryId) : BusinessException($"Category with id {categoryId} not found.")
{
    public long CategoryId { get; set; } = categoryId;
}
