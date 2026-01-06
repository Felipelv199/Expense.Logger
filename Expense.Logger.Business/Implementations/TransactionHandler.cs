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
    private readonly ICatgoriesRepository _catgoriesRepository = catgoriesRepository;

    private readonly ITransactionsRepository _transactionsRepository = transactionsRepository;

    public async Task<Transaction> CreateAsync(TransactionCreate create)
    {
        TransactionValidators.ValidateTransactionCreate(create);

        await EnsureTransactionCategoryExists(create.CategoryId);
        var transactionData = await _transactionsRepository.AddAndSave(create.ToDataModel());

        return transactionData.ToBusinessModel();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Transaction> GetByIdAsync(long id)
    {
        var transactionData = await _transactionsRepository.Find(id) ?? throw new TransactionNotFound(id);

        return transactionData.ToBusinessModel();
    }

    public async Task<PagedResponse<Transaction>> GetByPageAsync(TransactionQuery query)
    {
        TransactionValidators.ValidateTransactionQuery(query);

        var transactions = await _transactionsRepository.FindPageItems(query.ToTransactionPageInfo());

        return TransactionHelpers.BuildPagedTransactions(transactions, query);
    }

    public Task UpdateAsync(long id, TransactionUpdate transaction)
    {
        throw new NotImplementedException();
    }
}
