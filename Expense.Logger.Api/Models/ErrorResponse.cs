namespace Expense.Logger.Api.Models;

// API's error structure e.g. ProblemDetails
public class ErrorResponse
{
    public int Status { get; set; } // HTTP status code number

    public string Code { get; set; } // HTTP status code name

    public string Message { get; set; }

    public IEnumerable<ErrorDetail> Details { get; set; } = [];

    public string RequestId { get; set; } // unique-request-id-for-logging
}

public class ErrorDetail // field-specific error
{
    public string Field { get; set; }

    public string Issue { get; set; }
}