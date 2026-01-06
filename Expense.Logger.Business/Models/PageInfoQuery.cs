namespace Expense.Logger.Business.Models;

public class PageInfoQuery
{
    public int PageSize { get; set; } = 10;

    public string Search { get; set; }

    public long LastId { get; set; }
}
