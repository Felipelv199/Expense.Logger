using Expense.Logger.Business.Models.Transaction;
using Expense.Logger.Data.Models;

namespace Expense.Logger.Business.Mappers;

public static class TransactionMappers
{
    public static Transactions ToDataModel(this TransactionCreate transactionCreate) =>
        new ()
        {
            Amount = transactionCreate.Amount,
            CategoryId = transactionCreate.CategoryId,
            Date = transactionCreate.Date ?? throw new NullReferenceException(),
            Description = transactionCreate.Description,
            Name = transactionCreate.Name,
            Type = (int)transactionCreate.Type,
        };

    public static Transaction ToBusinessModel(this Transactions transactionData) =>
        new ()
        {
            Id = transactionData.TransactionId,
            Name = transactionData.Name,
            Amount = transactionData.Amount,
            Date = transactionData.Date,
            Category = transactionData.Category.ToBusinessModel(),
            Type = (TransactionType)transactionData.Type
        };
}
