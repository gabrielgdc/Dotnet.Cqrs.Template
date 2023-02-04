using Cqrs.Template.Domain.Exceptions;
using Cqrs.Template.Factories;
using Cqrs.Template.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Template.Controllers;

[Route("your-project-name/[controller]/v{version:apiVersion}")]
[ServiceFilter(typeof(GlobalExceptionFilterAttribute))]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
public abstract class BaseController : Controller
{
    private readonly ExceptionNotificationHandler _exceptionNotificationHandler;

    protected BaseController(INotificationHandler<ExceptionNotification> notifications)
    {
        _exceptionNotificationHandler = (ExceptionNotificationHandler)notifications;
    }

    private ProblemDetails GetProblem()
    {
        if (!_exceptionNotificationHandler.HasNotifications()) return default;

        return CustomProblemDetailsFactory.CreateProblemDetailsFromContext(
            HttpContext,
            _exceptionNotificationHandler
        );
    }

    protected IActionResult CreateResponse(IActionResult action)
    {
        var problem = GetProblem();

        if (problem is not null)
        {
            return new ObjectResult(problem)
            {
                StatusCode = problem.Status
            };
        }

        return action;
    }
}
