using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transaction;
using Expense.Logger.Business.Validators;

namespace Expense.Logger.Business.Implementations;

public class TransactionHandler : ITransactionsHandler
{
    public Task CreateAsync(TransactionCreate transactionCreate)
    {
        TransactionValidators.ValidateTransactionCreate(transactionCreate);

        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseItemsPaged<Transaction>> GetByPageAsync(TransactionQuery transactionQuery)
    {
        TransactionValidators.ValidateTransactionQuery(transactionQuery);

        throw new NotImplementedException();
    }

    public Task UpdateAsync(long id, TransactionUpdate transaction)
    {
        throw new NotImplementedException();
    }
}
