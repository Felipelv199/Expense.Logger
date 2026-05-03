using Expense.Logger.Data.Models;

namespace Expense.Logger.Data.Interfaces;

public interface ITransactionsRepository
{
    public Task<Transactions> AddAndSave(Transactions transactions);

    public Task<IEnumerable<Transactions>> FindByFilter(TransactionsFilter filter, Pagination pagination);
}
