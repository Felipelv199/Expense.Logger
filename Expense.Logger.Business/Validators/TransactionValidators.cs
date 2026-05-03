using Expense.Logger.Business.Models.Exceptions;
using Expense.Logger.Business.Models.Transaction;

namespace Expense.Logger.Business.Validators;

public static class TransactionValidators
{
    public static void ValidateTransactionCreate(TransactionCreate transaction)
    {
        if (transaction.Date > DateTime.Now)
        {
            throw new InvalidTransactionCreateException(nameof(transaction.Date), $"{nameof(transaction.Date)} cannot be in the future");
        }
    }

    public static void ValidateTransactionQuery(TransactionQuery query)
    {

        if (query.PageNumber < 1)
        {
            throw new InvalidTransactionQueryException(nameof(query.PageNumber), $"{nameof(query.PageNumber)} needs to be 1 or greater");
        }

        if (query.PageSize < 1)
        {
            throw new InvalidTransactionQueryException(nameof(query.PageSize), $"{nameof(query.PageSize)} needs to be 1 or greater");
        }

        if (query.EndDate is not null && query.EndDate is not null)
        {
            if (query.StartDate > query.EndDate)
            {
                throw new InvalidTransactionQueryException(nameof(query.StartDate), $"{nameof(query.EndDate)} needs to be greater than {nameof(query.StartDate)}");
            }
        }

        if (query.StartDate is not null)
        {
            if (query.StartDate > DateTime.Now)
            {
                throw new InvalidTransactionQueryException(nameof(query.StartDate), $"{nameof(query.StartDate)} needs to be less or equal to our current date and time");
            }
        }

        if (query.EndDate is not null)
        {
            if (query.EndDate > DateTime.Now)
            {
                throw new InvalidTransactionQueryException(nameof(query.EndDate), $"{query.EndDate} needs to be less or equal to our current date and time");
            }
        }

        if (query.MinAmount is not null && query.MaxAmount is not null)
        {
            if (query.MinAmount > query.MaxAmount)
            {
                throw new InvalidTransactionQueryException(nameof(query.MinAmount), $"{nameof(query.MinAmount)} needs to be less than or equal to {nameof(query.MaxAmount)}");
            }
        }
    }
}
