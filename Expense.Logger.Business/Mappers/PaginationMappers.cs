using Expense.Logger.Business.Models;
using Expense.Logger.Data.Models;

namespace Expense.Logger.Business.Mappers;

public static class PaginationMappers
{
    public static Pagination ToPagination(this PageInfoQuery pageInfoQuery) =>
        new()
        {
            Offset = (pageInfoQuery.PageNumber - 1) * pageInfoQuery.PageSize,
            Limit = pageInfoQuery.PageSize
        };
}
