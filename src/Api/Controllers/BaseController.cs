using Api.Factories;
using Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// Represents a base controller class providing common functionality and setup for API controllers.
/// </summary>
/// <remarks>
/// This abstract class establishes a foundation for building API controllers within the application. It incorporates
/// routing, exception handling, HTTP status code handling, dependency injection, and access to a mediator for
/// dispatching commands and queries.
/// </remarks>
[Route("[controller]/v{version:apiVersion}")]
[ServiceFilter(typeof(GlobalExceptionFilterAttribute))]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public abstract class BaseController(IMediator bus, ICustomProblemDetailsFactory customProblemDetailsFactory) : Controller
{
    /// <summary>
    /// A protected reference to the injected IMediator instance, used for dispatching commands and queries.
    /// </summary>
    protected readonly IMediator Bus = bus;
    
    /// <summary>
    /// A protected reference to the injected ICustomProblemDetailsFactory instance, used for creating problem details responses.
    /// </summary>
    protected readonly ICustomProblemDetailsFactory CustomProblemDetailsFactory = customProblemDetailsFactory;
}
