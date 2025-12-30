namespace Expense.Logger.Business.Models.Transaction;

public class TransactionUpdate
{
    public long Id { get; set; }

    public decimal Amount { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime Date {  get; set; }

    public long CategoryId { get; set; }

    public long TypeId { get; set; }
}
