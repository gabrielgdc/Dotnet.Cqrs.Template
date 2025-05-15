using Application.Shared.Resources;
using Application.Shared.ResultTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OneOf.Types;
using System.Linq;
using Error = Application.Shared.ResultTypes.Error;

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
    /// Creates a problem details model from a NotFound struct.
    /// </summary>
    /// <param name="notFound">A not found struct</param>
    IActionResult CreateProblemDetailsActionResult(NotFound notFound);

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
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error",
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
            Title = stringLocalizer[ErrorMessages.ValidationError],
            Detail = stringLocalizer[ErrorMessages.ValidationErrorDetail],
            Status = statusCode,
            Instance = _httpContext.Request.Path.ToString(),
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request",
            Extensions =
            {
                { "success", false },
                { "code", nameof(ErrorMessages.ValidationError) },
                { "traceId", _httpContext.TraceIdentifier },
                { "validationFailures", validationFailures }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    public IActionResult CreateProblemDetailsActionResult(NotFound notFound)
    {
        const int statusCode = StatusCodes.Status404NotFound;

        var problemDetails = new ProblemDetails
        {
            Title = stringLocalizer[ErrorMessages.ResourceNotFound],
            Detail = stringLocalizer[ErrorMessages.ResourceNotFoundDetail],
            Status = statusCode,
            Instance = _httpContext.Request.Path.ToString(),
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found",
            Extensions =
            {
                { "success", false },
                { "code", nameof(ErrorMessages.ResourceNotFound) },
                { "traceId", _httpContext.TraceIdentifier }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }

    public IActionResult CreateProblemDetailsActionResult()
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Title = stringLocalizer[ErrorMessages.UnhandledException],
            Detail = stringLocalizer[ErrorMessages.UnhandledExceptionDetail],
            Status = statusCode,
            Instance = _httpContext.Request.Path.ToString(),
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error",
            Extensions =
            {
                { "success", false },
                { "code", nameof(ErrorMessages.UnhandledException)},
                { "traceId", _httpContext.TraceIdentifier }
            }
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}