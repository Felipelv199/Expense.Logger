namespace Expense.Logger.Business.Models.Exceptions;

public class InvalidTransactionQueryException : BusinessException
{
    public InvalidTransactionQueryException(string fieldName, string description) : base("Invalid transaction create")
    {
        Details = new ExceptionFieldDetails()
        {
            FieldName = fieldName,
            Description = description
        };
    }

    public ExceptionFieldDetails Details { get; set; }
}
