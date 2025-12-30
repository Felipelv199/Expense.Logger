using Expense.Logger.Business.Interfaces;
using Expense.Logger.Business.Models;
using Expense.Logger.Business.Models.Transactions;

namespace Expense.Logger.Business.Implementations;

public class TransactionHandler : ITransactionsHandler
{
    public Task CreateAsync(TransactionCreate transaction)
    {
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

    public Task<ResponseItemsPaged<Transaction>> GetByPageAsync(TransactionRequestQuery query)
    {
        ValidateQuery(query);

        throw new NotImplementedException();
    }

    private static void ValidateQuery(TransactionRequestQuery query)
    {
        if (query.PageNumber is not null)
        {
            if (query.PageNumber < 1)
            {
                throw new Exception("Page number needs to be 1 or greater");
            }
        }

        if (query.PageSize is not null)
        {
            if(query.PageSize < 1)
            {
                throw new Exception("Page size needs to be 1 or greater");
            }
        }

        if (query.From is not null && query.To is not null)
        {
            if (query.From > query.To)
            {
                throw new Exception("To needs to be greater than From");
            }
        }

        if (query.From is not null)
        {
            if (query.From > DateTime.Now)
            {
                throw new Exception("From needs to be less or equal to our current date and time");
            }
        }

        if (query.To is not null)
        {
            if (query.To > DateTime.Now)
            {
                throw new Exception("To needs to be less or equal to our current date and time");
            }
        }
    }

    public Task UpdateAsync(long id, TransactionUpdate transaction)
    {
        throw new NotImplementedException();
    }
}
