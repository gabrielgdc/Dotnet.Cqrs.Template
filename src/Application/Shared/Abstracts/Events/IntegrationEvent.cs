using MediatR;
using System;

namespace Application.Shared.Abstracts.Events;

/// <summary>
/// Represents a base class for integration events within the MediatR framework.
/// </summary>
/// <remarks>
/// This abstract class serves as a foundation for defining events that signal significant occurrences within the application
/// and may need to be communicated to external systems for integration purposes. It inherits from the `INotification`
/// interface, enabling participation in the MediatR notification pattern for handling events.
/// 
/// Subclasses should inherit from this class and add specific properties and behavior relevant to their integration events.
/// </remarks>
public abstract record IntegrationEvent : INotification
{
    /// <summary>
    /// The UTC timestamp representing the time when the integration event occurred.
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}