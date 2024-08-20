using LightDiet.Recipe.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LightDiet.Recipe.Api.Filters;

public class ApiGlobalExceptionsFilter(IHostEnvironment env) : IExceptionFilter
{
    private readonly IHostEnvironment _env = env;

    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails();

        var exception = context.Exception;

        if (_env.IsDevelopment())
        {
            details.Extensions.Add("stackTrace", exception.StackTrace);
        }

        if (exception is EntityValidationException)
        {
            var returnedException = exception as EntityValidationException;

            details.Title = "One or more validation errors ocurred";
            details.Status = StatusCodes.Status422UnprocessableEntity;
            details.Type = "UnprocessableEntity";
            details.Detail = returnedException!.Message;
        }
        else
        {
            details.Title = "An unexpected error ocurred";
            details.Status = StatusCodes.Status500InternalServerError;
            details.Type = "UnexpectedError";
            details.Detail = exception!.Message;
        }

        context.HttpContext.Response.StatusCode = (int)details.Status;
        context.Result = new ObjectResult(details);
        context.ExceptionHandled = true;
    }
}
