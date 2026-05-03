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

    public async Task<IEnumerable<Transactions>> FindByFilter(TransactionsFilter filter, Pagination pagination)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var query = context.Transactions.AsQueryable();

        if (filter.StartDate is not null)
        {
            query = query.Where(t => t.Date >= filter.StartDate);
        }

        if (filter.EndDate is not null)
        {
            query = query.Where(t => t.Date <= filter.EndDate);
        }

        if (filter.MaxAmount is not null) 
        {
            query = query.Where(t => t.Amount <= filter.MaxAmount);
        }

        if (filter.MinAmount is not null)
        {
            query = query.Where(t => t.Amount >= filter.MinAmount);
        }

        if (filter.Type is not null)
        {
            query = query.Where(t => t.Type == filter.Type);
        }

        if (filter.Search is not null)
        {
            query = query.Where(t => t.Name.Contains(filter.Search) || t.Description.Contains(filter.Search));
        }

        return await query.OrderBy(t => t.CreatedAt).ThenBy(t => t.TransactionId).Skip(pagination.Offset)
            .Take(pagination.Limit).ToListAsync();
    }
}
