using System;
using Cqrs.Template.Dtos;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Cqrs.Template.Filters;

public class GlobalExceptionFilterAttribute : Attribute, IExceptionFilter
{
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly ILogger<GlobalExceptionFilterAttribute> _logger;

    public GlobalExceptionFilterAttribute(
        ApplicationConfiguration applicationConfiguration,
        ILogger<GlobalExceptionFilterAttribute> logger
    )
    {
        _applicationConfiguration = applicationConfiguration;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var eventId = new EventId(188, "GlobalException");

        _logger.LogError(eventId, context.Exception, context.Exception.Message);

        var errorResponse = new ErrorResponse(
            new[]
            {
                new Error(
                    _applicationConfiguration.GlobalErrorCode,
                    _applicationConfiguration.GlobalErrorMessage,
                    eventId,
                    context.HttpContext.Request.Path,
                    StatusCodes.Status500InternalServerError
                )
            }
        );
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}
