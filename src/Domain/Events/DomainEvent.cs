using MediatR;
using System;

namespace Domain.Events;

/// <summary>
/// Represents a base class for domain events within the Domain-Driven Design (DDD) pattern.
/// </summary>
/// <remarks>
/// This class serves as a foundation for defining domain events in your application. It implements the `INotification`
/// interface, enabling participation in the MediatR notification pattern for handling domain events. 
/// Subclasses should inherit from this class and add specific event-related properties.
/// 
/// Domain events capture significant changes within the domain and represent "what" happened without specifying "how".
/// They are typically published after domain entity changes (e.g., order placed, product updated) and can be used by
/// other parts of the application (UI, background processes) to react accordingly, implementing the "when" aspect.
/// </remarks>
public class DomainEvent : INotification
{
    /// <summary>
    /// The UTC timestamp of the domain event creation.
    /// </summary>
    public DateTime Timestamp { get; }

    protected DomainEvent()
    {
        Timestamp = DateTime.UtcNow;
    }
}