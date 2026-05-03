using Expense.Logger.Business.Models;

namespace Expense.Logger.Business.Helpers;

public static class PaginationHelper<T>
{
    public static ResponseItemsPaged<T> BuildResponseItemsPaged(IEnumerable<T> items, PageInfoQuery query) => new ()
        {
            Items = items,
            CurrentPage = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = items.Count(),
            TotalPages = (int) Math.Ceiling(items.Count() / (decimal) query.PageSize)
        };
}
