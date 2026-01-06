using Expense.Logger.Api.Mappers;
using Expense.Logger.Api.Models;
using Expense.Logger.Business.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using InvalidDataException = Expense.Logger.Business.Models.Exceptions.InvalidDataException;

namespace Expense.Logger.Api.Filters;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception == null)
        {
            return;
        }

        context.Result = new ObjectResult(BuildErrorResponse(context.Exception));
        context.ExceptionHandled = true;
    }

    private static ErrorResponse BuildErrorResponse(Exception exception)
    {
        var httpStatus = GetHttpStatus(exception);

        return new()
        {
            Status = (int)httpStatus,
            Code = httpStatus.ToString(),
            Message = httpStatus == HttpStatusCode.InternalServerError ? "The server was unable to complete your request. Please try again later." : exception.Message,
            RequestId = Guid.NewGuid().ToString(),
            Errors = httpStatus == HttpStatusCode.BadRequest && exception is InvalidDataException invalidDataException ? [invalidDataException.Details.ToErrorDetails()] : []
        };
    }

    private static HttpStatusCode GetHttpStatus(Exception exception) => exception switch
    {
        TransactionCategoryNotFound or TransactionCategoryNotFound or InvalidTransactionCreateException or InvalidTransactionQueryException => HttpStatusCode.BadRequest,

        TransactionNotFound => HttpStatusCode.NotFound,

        _ => HttpStatusCode.InternalServerError,
    };
}
