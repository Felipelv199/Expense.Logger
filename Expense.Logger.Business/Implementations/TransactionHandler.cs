using Expense.Logger.Business.Helpers;
using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Mappers;
using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Exceptions;
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

    public async Task<Transaction> GetByIdAsync(long id)
    {
        var transaction = await _transactionsRepository.FindById(id);

        return transaction is null ? throw new TransactionNotFound(id) : transaction.ToBusinessModel();
    }

    public async Task<ResponseItemsPaged<Transaction>> GetByPageAsync(TransactionQuery transactionQuery)
    {
        TransactionValidators.ValidateTransactionQuery(transactionQuery);
        var filter = transactionQuery.ToFilter();
        var transactions = await _transactionsRepository.FindByFilter(filter, transactionQuery.ToPagination());
        var totalCount = await _transactionsRepository.CountByFilter(filter);

        return PaginationHelper<Transaction>.BuildResponseItemsPaged(transactions.Select(t => t.ToBusinessModel()), transactionQuery, totalCount);
    }

    public Task UpdateAsync(long id, TransactionUpdate transaction)
    {
        throw new NotImplementedException();
    }
}
