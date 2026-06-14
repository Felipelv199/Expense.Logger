namespace Expense.Logger.Data.Models;

public class TransactionsFilter
{
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? MinAmount { get; set; }

    public decimal? MaxAmount { get; set; }

    public int? Type { get; set; }

    public string Search { get; set; }
}
