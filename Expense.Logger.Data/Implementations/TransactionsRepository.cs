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

    public async Task<Transactions?> FindById(long id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Transactions
            .Include(t => t.Category)
            .Include(t => t.BankAccount)
            .FirstOrDefaultAsync(t => t.TransactionId == id);
    }

    public async Task<IEnumerable<Transactions>> FindByFilter(TransactionsFilter filter, Pagination pagination)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await ApplyFilter(context.Transactions.AsQueryable(), filter)
            .Include(t => t.Category)
            .Include(t => t.BankAccount)
            .OrderBy(t => t.CreatedAt).ThenBy(t => t.TransactionId)
            .Skip(pagination.Offset).Take(pagination.Limit).ToListAsync();
    }

    public async Task<long> CountByFilter(TransactionsFilter filter)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await ApplyFilter(context.Transactions.AsQueryable(), filter).LongCountAsync();
    }

    private static IQueryable<Transactions> ApplyFilter(IQueryable<Transactions> query, TransactionsFilter filter)
    {
        if (filter.StartDate is not null)
            query = query.Where(t => t.Date >= filter.StartDate);

        if (filter.EndDate is not null)
            query = query.Where(t => t.Date <= filter.EndDate);

        if (filter.MaxAmount is not null)
            query = query.Where(t => t.Amount <= filter.MaxAmount);

        if (filter.MinAmount is not null)
            query = query.Where(t => t.Amount >= filter.MinAmount);

        if (filter.Type is not null)
            query = query.Where(t => t.Type == filter.Type);

        if (filter.Search is not null)
            query = query.Where(t => 
                (t.Name != null && t.Name.Contains(filter.Search)) ||
                (t.Description != null && t.Description.Contains(filter.Search))
            );

        return query;
    }
}
