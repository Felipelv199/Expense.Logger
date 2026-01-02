using Expense.Logger.Business.Models;
using Expense.Logger.Data.Models;

namespace Expense.Logger.Business.Mappers;

public static class CategoryMappers
{
    public static Category ToBusinessModel(this Categories categories) => new ()
    {
        Id = categories.CategoryId,
        Name = categories.Name
    };
}
