using System;
using Api.Factories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Api.Filters;

public class GlobalExceptionFilterAttribute(ILogger logger) : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var eventId = new EventId(188, "GlobalException");

        logger.LogError(eventId, context.Exception, context.Exception.Message);

        context.Result = CustomProblemDetailsFactory.CreateProblemDetailsActionResult(context.HttpContext);
    }
}
