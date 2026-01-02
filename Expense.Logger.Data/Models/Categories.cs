using Microsoft.EntityFrameworkCore;

namespace Expense.Logger.Data.Models;

[PrimaryKey(nameof(CategoryId))]
public class Categories : BaseDataModel
{
    public long CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Transactions> Transactions { get; set; } = [];
}
