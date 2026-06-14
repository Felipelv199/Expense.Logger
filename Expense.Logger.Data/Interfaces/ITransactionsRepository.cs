using Expense.Logger.Data.Models;

namespace Expense.Logger.Data.Interfaces;

public interface ITransactionsRepository
{
    public Task<Transactions> AddAndSave(Transactions transactions);

    public Task<Transactions?> FindById(long id);

    public Task<IEnumerable<Transactions>> FindByFilter(TransactionsFilter filter, Pagination pagination);

    public Task<long> CountByFilter(TransactionsFilter filter);
}
