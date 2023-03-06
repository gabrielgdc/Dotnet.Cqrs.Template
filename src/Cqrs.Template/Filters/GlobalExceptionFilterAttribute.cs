using System;
using Cqrs.Template.Factories;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cqrs.Template.Filters;

public class GlobalExceptionFilterAttribute : Attribute, IExceptionFilter
{
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly ILogger<GlobalExceptionFilterAttribute> _logger;

    public GlobalExceptionFilterAttribute(
        IOptions<ApplicationConfiguration> applicationConfiguration,
        ILogger<GlobalExceptionFilterAttribute> logger
    )
    {
        _applicationConfiguration = applicationConfiguration.Value;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var eventId = new EventId(188, "GlobalException");

        _logger.LogError(eventId, context.Exception, context.Exception.Message);

        var problemDetails = CustomProblemDetailsFactory.CreateProblemDetailsFromContext(
            context.HttpContext,
            _applicationConfiguration.GlobalErrorMessage,
            _applicationConfiguration.GlobalErrorCode
        );

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
