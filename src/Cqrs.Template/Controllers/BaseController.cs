using Cqrs.Template.Domain.Exceptions;
using Cqrs.Template.Dtos;
using Cqrs.Template.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Template.Controllers;

[Route("your-project-name/[controller]/v{version:apiVersion}")]
[ServiceFilter(typeof(GlobalExceptionFilterAttribute))]
public abstract class BaseController : Controller
{
    private readonly ExceptionNotificationHandler _notifications;

    protected BaseController(INotificationHandler<ExceptionNotification> notifications)
    {
        _notifications = (ExceptionNotificationHandler)notifications;
    }

    private bool IsValidOperation()
    {
        return !_notifications.HasNotifications();
    }

    protected IActionResult CreateResponse(IActionResult action)
    {
        if (!IsValidOperation())
        {
            return BadRequest(new Response<object>(
                _notifications.GetNotifications())
            );
        }

        return action;
    }
}
