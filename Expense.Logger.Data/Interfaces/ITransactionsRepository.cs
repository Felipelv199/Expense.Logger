using Expense.Logger.Data.Models;

namespace Expense.Logger.Data.Interfaces;

public interface ITransactionsRepository
{
    Task<Transactions> AddAndSave(Transactions transactions);

    Task<Transactions> Find(long id);

    Task<IEnumerable<Transactions>> FindPageItems(TransactionPageInfo transactionPageInfo);
}
