using System;
using Microsoft.Extensions.Logging;

namespace Cqrs.Template.Dtos;

public class ErrorResponse
{
    public bool Success { get; }
    public Error[] Errors { get; }

    public ErrorResponse(Error[] errors)
    {
        Success = false;
        Errors = errors;
    }
}

public class Error
{
    public string Code { get; }
    public string Message { get; }
    public EventId EventId { get; }
    public string Instance { get; }
    public int Status { get; }
    public DateTime TimeStamp { get; }

    public Error(string code, string message, EventId eventId, string instance, int status)
    {
        Code = code;
        Message = message;
        EventId = eventId;
        Instance = instance;
        Status = status;
        TimeStamp = DateTime.UtcNow;
    }
}
