using System.Text.Json.Serialization;

namespace Expense.Logger.Business.Models.Transaction;

public class Transaction
{
    public long Id { get; set; }

    public string Name { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Category Category { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType Type { get; set; }
}