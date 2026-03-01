namespace API.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            ProductDataException => StatusCodes.Status502BadGateway,
            NoProductsException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Result = new ObjectResult(new ProblemDetails
        {
            Status = statusCode,
            Detail = context.Exception.Message,
        })
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}
