using Cqrs.Template.Domain.Enums;
using Cqrs.Template.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Template.Factories;

public static class CustomProblemDetailsFactory
{
    private const string ClientErrorDetail = "Ocorreu um erro com a sua requisição, verique os parâmetros obrigatórios";
    private const string ServerErrorDetail = "Ocorreu um erro interno, tente novamente mais tarde";

    public static ProblemDetails CreateProblemDetailsFromContext(HttpContext httpContext, ExceptionNotificationHandler exceptionHandler)
    {
        var isClientError = exceptionHandler.GetExceptionType() is ExceptionType.Client;

        return new ProblemDetails
        {
            Title = "Erro ao processar sua requisição",
            Detail = isClientError ? ClientErrorDetail : ServerErrorDetail,
            Status = isClientError ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError,
            Instance = httpContext.Request.Path.ToString(),
            Extensions =
            {
                { "success", false },
                { "traceId", httpContext.TraceIdentifier },
                { "errors", exceptionHandler.GetNotifications() }
            }
        };
    }

    public static ProblemDetails CreateProblemDetailsFromContext(HttpContext httpContext, string detail, string code)
    {
        return new ProblemDetails
        {
            Title = "Erro ao processar sua requisição",
            Detail = detail,
            Status = StatusCodes.Status500InternalServerError,
            Instance = httpContext.Request.Path.ToString(),
            Extensions =
            {
                { "success", false },
                { "traceId", httpContext.TraceIdentifier },
                { "code", code }
            }
        };
    }
}
