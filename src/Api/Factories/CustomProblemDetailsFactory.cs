using System.Linq;
using Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Api.Factories;

/// <summary>
/// Interface for an custom problem details factory.
/// </summary>
/// <see href="https://datatracker.ietf.org/doc/html/rfc7807">a</see>
public interface ICustomProblemDetailsFactory
{
    /// <summary>
    /// Creates a problem details model action result from a Error object.
    /// </summary>
    /// <returns></returns>
    /// <param name="error">A error object containing all errors</param>
    IActionResult CreateProblemDetailsActionResult(Error error);
    
    /// <summary>
    /// Creates a problem details model from a ValidationFailed object.
    /// </summary>
    /// <param name="validationFailed">A validation failed containing all failed validations</param>
    IActionResult CreateProblemDetailsActionResult(ValidationFailed validationFailed);
    
    /// <summary>
    /// Creates a generic problem details model.
    /// </summary>
    /// <returns>IActionResult</returns>
    IActionResult CreateProblemDetailsActionResult();
}

/// <inheritdoc />
public class CustomProblemDetailsFactory(IHttpContextAccessor httpContextAccessor, IStringLocalizer<CustomProblemDetailsFactory> stringLocalizer) : ICustomProblemDetailsFactory
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext;
    
    public IActionResult CreateProblemDetailsActionResult(Error error)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = error.Title,
            Detail = error.Detail,
            Status = statusCode,
            Instance = _httpContext.Request.Path.ToString(),
            Type = "about:blank",
            Extensions =
            {
                { "success", false },
                { "code", error.Code },
                { "traceId", _httpContext.TraceIdentifier }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    public IActionResult CreateProblemDetailsActionResult(ValidationFailed validationFailed)
    {
        const int statusCode = StatusCodes.Status400BadRequest;

        var validationFailures = validationFailed.ValidationFailures.Select(v => new { v.ErrorCode, v.ErrorMessage, v.PropertyName });

        var problemDetails = new ProblemDetails
        {
            Title = stringLocalizer[ProblemDetailsErrorMessages.ValidationError],
            Detail = stringLocalizer[ProblemDetailsErrorMessages.ValidationErrorDetail],
            Status = statusCode,
            Instance = _httpContext.Request.Path.ToString(),
            Type = "about:blank",
            Extensions =
            {
                { "success", false },
                { "code", nameof(ProblemDetailsErrorMessages.ValidationError) },
                { "traceId", _httpContext.TraceIdentifier },
                { "validationFailures", validationFailures }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    public IActionResult CreateProblemDetailsActionResult()
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = stringLocalizer[ProblemDetailsErrorMessages.UnhandledException],
            Detail = stringLocalizer[ProblemDetailsErrorMessages.UnhandledExceptionDetail],
            Status = statusCode,
            Instance = _httpContext.Request.Path.ToString(),
            Extensions =
            {
                { "success", false },
                { "code", nameof(ProblemDetailsErrorMessages.UnhandledException)},
                { "traceId", _httpContext.TraceIdentifier }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}
