using Expense.Logger.Business.Models;

namespace Expense.Logger.Business.Helpers;

public static class PaginationHelper<T>
{
    public static ResponseItemsPaged<T> BuildResponseItemsPaged(IEnumerable<T> items, PageInfoQuery query, long totalCount) => new()
        {
            Items = items,
            CurrentPage = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (decimal)query.PageSize)
        };
}
