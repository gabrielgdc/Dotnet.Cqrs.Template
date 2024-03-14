using System;
using Api.Factories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Api.Filters;

/// <summary>
/// Represents a global exception filter attribute that handles unhandled exceptions within the application.
/// </summary>
/// <remarks>
/// This class acts as a catch-all mechanism for exceptions that aren't handled within specific controllers or actions.
/// It logs the exception details for later analysis and returns a standardized problem detail response to the client.
/// </remarks>
public class GlobalExceptionFilterAttribute(ICustomProblemDetailsFactory customProblemDetailsFactory, ILogger logger) : Attribute, IExceptionFilter
{
    /// <summary>
    /// Logs the exception details and creates a problem details response when an unhandled exception occurs.
    /// </summary>
    /// <param name="context">The exception context, containing information about the exception and the current action.</param>
    public void OnException(ExceptionContext context)
    {
        var eventId = new EventId(188, "GlobalException");

        logger.LogError(eventId, context.Exception, context.Exception.Message);

        context.Result = customProblemDetailsFactory.CreateProblemDetailsActionResult();
    }
}
