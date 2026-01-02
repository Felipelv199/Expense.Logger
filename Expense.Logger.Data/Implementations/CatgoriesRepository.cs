using Expense.Logger.Data.Interfaces;
using Expense.Logger.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense.Logger.Data.Implementations;

public class CatgoriesRepository(IDbContextFactory<ExpenseLoggerDbContext> contextFactory) : ICatgoriesRepository
{
    public IDbContextFactory<ExpenseLoggerDbContext> _contextFactory = contextFactory;

    public async Task<Categories> Find(long categoryId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Categories.FindAsync(categoryId);
    }
}
