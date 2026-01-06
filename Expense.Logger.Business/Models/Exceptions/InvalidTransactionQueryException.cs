namespace Expense.Logger.Business.Models.Exceptions;

public class InvalidTransactionQueryException(string fieldName, string description)
    : InvalidDataException("Invalid transaction query", fieldName, description) { }
