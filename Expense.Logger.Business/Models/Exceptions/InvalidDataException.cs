namespace Expense.Logger.Business.Models.Exceptions;

public class InvalidDataException : BusinessException
{
    public InvalidDataException(string message, string fieldName, string description) : base(message)
    {
        Details = new ExceptionFieldDetails()
        {
            FieldName = fieldName,
            Description = description
        };
    }

    public ExceptionFieldDetails Details { get; set; }
}
