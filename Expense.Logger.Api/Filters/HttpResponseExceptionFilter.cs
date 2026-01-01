using Expense.Logger.Api.Models;
using Expense.Logger.Business.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

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
            Message = GetErrorMessage(exception),
            RequestId = Guid.NewGuid().ToString()
        };
    }

    private static HttpStatusCode GetHttpStatus(Exception exception) => exception switch
    {
        TransactionCategoryNotFound transactionCategoryNotFound => HttpStatusCode.BadRequest,

        _ => HttpStatusCode.InternalServerError,
    };

    private static string GetErrorMessage(Exception exception) => exception switch
    {
        TransactionCategoryNotFound transactionCategoryNotFound => transactionCategoryNotFound.Message,

        _ => "The server was unable to complete your request. Please try again later."
    };
}
