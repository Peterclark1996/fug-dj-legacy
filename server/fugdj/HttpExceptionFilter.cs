using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace fugdj;

public class HttpExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not HttpException exception) return;
            
        context.Result = new ObjectResult(exception.Message) { StatusCode = exception.StatusCode };
        context.ExceptionHandled = true;
    }
}