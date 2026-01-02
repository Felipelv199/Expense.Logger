using Expense.Logger.Business.Models.Exceptions;
using Expense.Logger.Business.Models.Transaction;

namespace Expense.Logger.Business.Validators;

public static class TransactionValidators
{
    public static void ValidateTransactionCreate(TransactionCreate transaction)
    {
        if (transaction.Date > DateTime.Now)
        {
            throw new InvalidTransactionCreateException(nameof(transaction.Date), "Transaction date cannot be in the future");
        }
    }

    public static void ValidateTransactionQuery(TransactionQuery query)
    {
        if (query.PageNumber is not null)
        {
            if (query.PageNumber < 1)
            {
                throw new InvalidTransactionQueryException(nameof(query.PageNumber), "Page number needs to be 1 or greater");
            }
        }

        if (query.PageSize is not null)
        {
            if (query.PageSize < 1)
            {
                throw new InvalidTransactionQueryException(nameof(query.PageSize), "Page size needs to be 1 or greater");
            }
        }

        if (query.From is not null && query.To is not null)
        {
            if (query.From > query.To)
            {
                throw new InvalidTransactionQueryException(nameof(query.To), "To needs to be greater than From");
            }
        }

        if (query.From is not null)
        {
            if (query.From > DateTime.Now)
            {
                throw new InvalidTransactionQueryException(nameof(query.From), "From needs to be less or equal to our current date and time");
            }
        }

        if (query.To is not null)
        {
            if (query.To > DateTime.Now)
            {
                throw new InvalidTransactionQueryException(nameof(query.To), "To needs to be less or equal to our current date and time");
            }
        }
    }
}
