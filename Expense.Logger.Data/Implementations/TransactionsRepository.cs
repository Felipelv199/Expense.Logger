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
            await context.Entry(entity.Entity).Reference(entity => entity.Category).LoadAsync();

        return entity.Entity;
    }

    public async Task<Transactions> Find(long id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var entity = await context.Transactions
            .FindAsync(id);

        if (entity is null)
            return null;

        if (entity.CategoryId is not null)
            await context.Entry(entity).Reference(e => e.Category).LoadAsync();

        return entity;
    }

    public async Task<IEnumerable<Transactions>> FindPageItems(TransactionPageInfo pageInfo)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        ArgumentNullException.ThrowIfNull(pageInfo);

        var query = context.Transactions.AsQueryable();

        if (pageInfo.From is not null)
            query = query.Where(transactions => transactions.Date >= pageInfo.From.Value);

        if (pageInfo.To is not null)
            query = query.Where(transactions => transactions.Date <= pageInfo.To.Value);

        return await query.OrderBy(transactions => transactions.Date).ThenBy(transactions => transactions.TransactionId)
            .Where(transactions => transactions.Date < pageInfo.LastDate || (transactions.Date == pageInfo.LastDate &&
                pageInfo.LastTransactionId.HasValue && transactions.TransactionId > pageInfo.LastTransactionId.Value))
            .Take(pageInfo.Take).ToListAsync();
    }
}
