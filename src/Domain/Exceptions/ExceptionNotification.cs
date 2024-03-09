using System.Text.Json.Serialization;
using Domain.Events;

namespace Domain.Exceptions;

public class ExceptionNotification(string code, string message, ExceptionType exceptionType, string paramName = null)
    : DomainEvent
{
    public string Code { get; } = code;
    public string Message { get; } = message;
    public string ParamName { get; } = paramName;

    [JsonIgnore] public ExceptionType Type { get; } = exceptionType;
}
