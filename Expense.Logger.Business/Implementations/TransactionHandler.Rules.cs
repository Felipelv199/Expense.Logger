using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Models.Exceptions;

namespace Expense.Logger.Business.Implementations;

public partial class TransactionHandler : ITransactionsHandler
{
    private async Task EnsureTransactionCategoryExists(long? categoryId)
    {
        if (categoryId is null)
            return;

        var category = await _catgoriesRepository.Find(categoryId.Value);

        if (category is not null)
            return;

        throw new TransactionCategoryNotFound(categoryId.Value);
    }
}
