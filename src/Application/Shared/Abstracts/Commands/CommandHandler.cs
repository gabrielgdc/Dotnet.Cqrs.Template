using Domain.SeedWork;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OneOf;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Shared.Abstracts.Commands;

/// <summary>
/// Represents a base class for command handlers within the MediatR library, integrating with logging, localization, 
/// and unit of work patterns for persistence operations.
/// </summary>
/// <typeparam name="TRequest">The type of command request that this handler is responsible for processing. 
/// This type must inherit from the Command base class.
/// </typeparam>
/// <typeparam name="TResponse">The type of response that the handler will produce, implementing the `IOneOf` interface 
/// to represent various potential outcomes (success, failure, validation errors, etc.).</typeparam>
/// <remarks>
/// This abstract class provides a foundation for implementing MediatR command handlers in a structured way. 
/// It incorporates logging, localization, and unit of work management for consistency across command handlers.
/// 
/// Subclasses should inherit from this class and implement the `Handle` method to define the specific logic for handling 
/// the associated command request. They can access the provided logging, localization, and unit of work services for 
/// their operations.
/// </remarks>
public abstract class CommandHandler<TRequest, TResponse>(
    IUnitOfWork uow,
    ILogger logger,
    IStringLocalizer<CommandHandler<TRequest, TResponse>> stringLocalizer)
    : IRequestHandler<TRequest, TResponse>
    where TRequest : Command<TResponse>
    where TResponse : IOneOf
{
    /// <summary>
    /// The logger instance used for logging messages within the command handler.
    /// </summary>
    protected readonly ILogger Logger = logger;

    /// <summary>
    /// The string localizer instance used for retrieving localized strings within the command handler.
    /// </summary>
    protected readonly IStringLocalizer<CommandHandler<TRequest, TResponse>> StringLocalizer = stringLocalizer;

    /// <summary>
    /// Attempts to commit any pending changes to the database within the current unit of work.
    /// </summary>
    /// <returns>True if the commit was successful, false otherwise. Logs a critical error message if the commit fails.</returns>
    public async Task<bool> CommitAsync()
    {
        if (await uow.CommitAsync()) return true;

        Logger.LogCritical("Problem on saving changes in database");

        return false;
    }

    /// <summary>
    /// Handles the execution of the specified command request. 
    /// </summary>
    /// <param name="request">The command request to be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to signal cancellation of the operation.</param>
    /// <returns>A Task representing the asynchronous operation that produces a response of type TResponse.</returns>
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}