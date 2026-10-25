using Expense.Logger.Business.Models;
using Expense.Logger.Data.Models;

namespace Expense.Logger.Business.Mappers;

public static class BankAccountMappers
{
    public static BankAccount ToBusinessModel(this BankAccounts bankAccounts) => new()
    {
        Id = bankAccounts.BankAccountId,
        Name = bankAccounts.Name
    };
}
