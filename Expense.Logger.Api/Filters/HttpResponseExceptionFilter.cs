using Expense.Logger.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

    private static ErrorResponse BuildErrorResponse(Exception exception) => new()
    {
        Status = 500,
        Code = "InternalServerError",
        Message = exception.Message,
        RequestId = Guid.NewGuid().ToString()
    };
}
