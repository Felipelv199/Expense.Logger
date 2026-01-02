using Expense.Logger.Data.Interfaces;
using Expense.Logger.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Expense.Logger.Data.Implementations;

public class TransactionsRepository(IDbContextFactory<ExpenseLoggerDbContext> contextFactory) : ITransactionsRepository
{
    public IDbContextFactory<ExpenseLoggerDbContext> _contextFactory = contextFactory;

    public async Task<Transactions> AddAndSave(Transactions transactions)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var entity = await context.Transactions.AddAsync(transactions);
        await context.SaveChangesAsync();

        if (entity.Entity.CategoryId is not null)
        {
            await context.Entry(entity.Entity).Reference(entity => entity.Category).LoadAsync();
        }

        return entity.Entity;
    }
}
