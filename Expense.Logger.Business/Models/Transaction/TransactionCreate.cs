using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Expense.Logger.Business.Models.Transaction;

public class TransactionCreate
{
    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public DateTime? Date {  get; set; }

    public long CategoryId { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionType? Type { get; set; }
}
