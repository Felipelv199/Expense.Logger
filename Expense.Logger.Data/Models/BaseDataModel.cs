using System.ComponentModel.DataAnnotations.Schema;

namespace Expense.Logger.Data.Models;

public abstract class BaseDataModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
}
