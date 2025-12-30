using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transaction;
using TransactionBusiness = Expense.Logger.Business.Models.Transaction.Transaction;

namespace Expense.Logger.Business.Interfaces;

public interface ITransactionsHandler
{
    public Task<ResponseItemsPaged<TransactionBusiness>> GetByPageAsync(TransactionQuery query);

    public Task<TransactionBusiness> GetByIdAsync(long id);

    public Task CreateAsync(TransactionCreate transaction);

    public Task UpdateAsync(long id, TransactionUpdate transaction);

    public Task DeleteAsync(long id);
}
