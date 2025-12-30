using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transactions;
using TransactionBusiness = Expense.Logger.Business.Models.Transactions.Transaction;

namespace Expense.Logger.Business.Interfaces;

public interface ITransactionsHandler
{
    public Task<ResponseItemsPaged<TransactionBusiness>> GetByPageAsync(TransactionRequestQuery query);

    public Task<TransactionBusiness> GetByIdAsync(long id);

    public Task CreateAsync(TransactionCreate transaction);

    public Task UpdateAsync(long id, TransactionUpdate transaction);

    public Task DeleteAsync(long id);
}
