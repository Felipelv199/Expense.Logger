using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transaction;

namespace Expense.Logger.Business.Interfaces;

public interface ITransactionsHandler
{
    public Task<ResponseItemsPaged<Transaction>> GetByPageAsync(TransactionQuery query);

    public Task<Transaction> GetByIdAsync(long id);

    public Task<Transaction> CreateAsync(TransactionCreate transaction);

    public Task UpdateAsync(long id, TransactionUpdate transaction);

    public Task DeleteAsync(long id);
}
