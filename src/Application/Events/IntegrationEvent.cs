using System;
using MediatR;

namespace Application.Events;

public abstract class IntegrationEvent : INotification
{
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
}
