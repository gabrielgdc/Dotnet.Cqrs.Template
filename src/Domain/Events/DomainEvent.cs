using System;
using MediatR;

namespace Domain.Events;

public class DomainEvent : INotification
{
    public DateTime Timestamp { get; }

    protected DomainEvent()
    {
        Timestamp = DateTime.UtcNow;
    }
}
