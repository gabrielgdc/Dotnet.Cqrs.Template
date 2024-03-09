using System.Linq;
using Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Factories;

public static class CustomProblemDetailsFactory
{
    public static IActionResult CreateProblemDetailsActionResult(this Error error, HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = error.Title,
            Detail = error.Detail,
            Status = statusCode,
            Instance = httpContext.Request.Path.ToString(),
            Type = "about:blank",
            Extensions =
            {
                { "success", false },
                { "code", error.Code },
                { "traceId", httpContext.TraceIdentifier }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    public static IActionResult CreateProblemDetailsActionResult(this ValidationFailed validationFailed, HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status400BadRequest;

        var validationFailures = validationFailed.ValidationFailures.Select(v => new { v.ErrorCode, v.ErrorMessage, v.PropertyName });

        var problemDetails = new ProblemDetails
        {
            Title = "Validation failed.",
            Detail = "Refers to the Validation Failures property to more information.",
            Status = statusCode,
            Instance = httpContext.Request.Path.ToString(),
            Type = "about:blank",
            Extensions =
            {
                { "success", false },
                { "code", "ValidationFailed" },
                { "traceId", httpContext.TraceIdentifier },
                { "validationFailures", validationFailures }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    public static IActionResult CreateProblemDetailsActionResult(HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = "It was not possible to process your request.",
            Detail = "An internal error occurred while processing your request.",
            Status = statusCode,
            Instance = httpContext.Request.Path.ToString(),
            Extensions =
            {
                { "success", false },
                { "code", "UnhandledException"},
                { "traceId", httpContext.TraceIdentifier }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}
