using Expense.Logger.Business.Mappers;
using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transaction;
using Expense.Logger.Data.Models;

namespace Expense.Logger.Business.Helpers;

public static class TransactionHelpers
{
    public static PagedResponse<Transaction> BuildPagedTransactions(IEnumerable<Transactions> transactions, TransactionQuery query)
    {
        var items = transactions.Select(t => t.ToBusinessModel()).ToList();
        var lastItem = items.LastOrDefault();

        if (lastItem == null)
            return new PagedResponse<Transaction>
            {
                Items = [],
                PageSize = query.PageSize
            };
        

        var pageData = new TransactionQueryPageData()
        {
            NextTransactionId = lastItem.Id,
            NextDate = lastItem.Date,
        };
        bool hasNextPage = items.Count == query.PageSize;

        return new PagedResponse<Transaction>
        {
            Items = items,
            NextPageKey = hasNextPage ? pageData.ToStringKey() : null,
            PageSize = query.PageSize,
            HasNextPage = hasNextPage
        };
    }
}
