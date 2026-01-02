using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense.Logger.Data.Models;

[PrimaryKey(nameof(BankAccountId))]
[Table("bank_accounts")]
public class BankAccounts : BaseDataModel
{
    public long BankAccountId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Transactions> Transactions { get; set; } = [];
}
