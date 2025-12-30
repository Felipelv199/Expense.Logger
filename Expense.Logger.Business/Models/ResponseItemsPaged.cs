namespace Expense.Logger.Business.Models;

public class ResponseItemsPaged<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public long TotalCount { get; set; }

    public int PageSize { get; set; }

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }
}
