using Expense.Logger.Api.Models;
using Expense.Logger.Business.Models.Exceptions;

namespace Expense.Logger.Api.Mappers;

public static class ExceptionMappers
{
    public static ErrorDetails ToErrorDetails(this ExceptionFieldDetails details) =>
        new()
        {
            Field = details.FieldName,
            Issue = details.Description
        };
}
