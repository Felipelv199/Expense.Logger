namespace Expense.Logger.Business.Models;

public class PageInfoQuery
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string Search { get; set; }
}
