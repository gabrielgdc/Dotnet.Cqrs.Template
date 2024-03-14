using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.Events;

/// <summary>
/// Represents a base class for event handlers within the MediatR framework.
/// </summary>
/// <typeparam name="T">The type of notification that this handler is responsible for processing.</typeparam>
/// <remarks>
/// This abstract class provides a foundation for implementing MediatR event handlers that respond to notifications 
/// (events) within the application. It inherits from the `INotificationHandler` interface, ensuring consistency
/// with MediatR's notification handling mechanisms.
/// 
/// Subclasses should inherit from this class and implement the `Handle` method to define the specific logic to
/// execute when the associated notification is received.
/// </remarks>
public abstract class EventHandler<T> : INotificationHandler<T> where T : INotification
{
    /// <summary>
    /// Handles the execution of the specified notification.
    /// </summary>
    /// <param name="notification">The notification object to be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to signal cancellation of the operation.</param>
    /// <returns>A Task representing the asynchronous operation that performs the event handling logic.</returns>
    public abstract Task Handle(T notification, CancellationToken cancellationToken);
}
