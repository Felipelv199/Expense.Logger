using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Mappers;
using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transaction;
using Expense.Logger.Business.Validators;
using Expense.Logger.Data.Interfaces;

namespace Expense.Logger.Business.Implementations;

public partial class TransactionHandler(ICatgoriesRepository catgoriesRepository, ITransactionsRepository transactionsRepository) : ITransactionsHandler
{
    public ICatgoriesRepository _catgoriesRepository = catgoriesRepository;

    public ITransactionsRepository _transactionsRepository = transactionsRepository;

    public async Task<Transaction> CreateAsync(TransactionCreate transactionCreate)
    {
        TransactionValidators.ValidateTransactionCreate(transactionCreate);

        await EnsureTransactionCategoryExists(transactionCreate.CategoryId);
        var transactionData = await _transactionsRepository.AddAndSave(transactionCreate.ToDataModel());

        return transactionData.ToBusinessModel();
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
