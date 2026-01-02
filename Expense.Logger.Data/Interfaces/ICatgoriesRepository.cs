using Expense.Logger.Data.Models;

namespace Expense.Logger.Data.Interfaces;

public interface ICatgoriesRepository
{
    Task<Categories> Find(long categoryId);
}
