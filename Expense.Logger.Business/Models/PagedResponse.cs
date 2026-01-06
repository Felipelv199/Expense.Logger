namespace Expense.Logger.Business.Models;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public string NextPageKey { get; set; }

    public int PageSize { get; set; }

    public bool HasNextPage { get; set; }
}