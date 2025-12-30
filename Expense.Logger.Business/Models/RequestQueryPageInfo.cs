namespace Expense.Logger.Business.Models;

public class RequestQueryPageInfo
{
    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string Search { get; set; }
}
