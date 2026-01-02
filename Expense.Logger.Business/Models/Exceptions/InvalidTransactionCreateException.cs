namespace Expense.Logger.Business.Models.Exceptions;

public class InvalidTransactionCreateException(string fieldName, string description)
    : InvalidDataException("Invalid transaction create", fieldName, description) { }