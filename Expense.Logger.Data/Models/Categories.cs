using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense.Logger.Data.Models;

[PrimaryKey(nameof(CategoryId))]
public class Categories
{
    public long CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<Transactions> Transactions { get; set; } = [];
}
