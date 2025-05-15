using Application.Features.SampleQuery.Models;
using Application.Shared.Abstracts.Queries;
using Application.Shared.Resources;
using Application.Shared.ResultTypes;
using Infra.CrossCutting.Environments.Configurations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OneOf.Types;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Error = Application.Shared.ResultTypes.Error;

namespace Application.Features.SampleQuery;

/// <summary>
/// Represents a query handler responsible for executing the SampleQuery and returning either sample data or appropriate error responses.
/// </summary>
public class SampleQueryHandler(
    IStringLocalizer<SampleQueryHandler> stringLocalizer,
    DatabaseConfiguration databaseConfiguration,
    ILogger logger)
    : QueryHandler<SampleQuery, SampleQueryResults>(databaseConfiguration, logger, stringLocalizer)
{
    /// <summary>
    /// A collection of sample strings for demonstration purposes.
    /// </summary>
    private static readonly string[] Samples = ["Sample Query Execution", "Lorem Ipsum is simply dummy text"];

    /// <summary>
    /// Handles the execution of the SampleQuery, performing validation, simulating potential errors, and returning query results or error responses.
    /// </summary>
    /// <param name="request">The SampleQuery object containing query parameters.</param>
    /// <param name="cancellationToken">A cancellation token to signal cancellation of the operation.</param>
    /// <returns>A Task representing the asynchronous operation that produces a SampleQueryResult object containing either sample data or error information.</returns>
    public override async Task<SampleQueryResults> Handle(SampleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!request.IsValid()) return new ValidationFailed(request.ValidationResult.Errors);

            await Task.Yield();

            if (request.ForceClientException)
            {
                return new Error(
                    nameof(ErrorMessages.ExceptionForced),
                    StringLocalizer[ErrorMessages.ExceptionForced],
                    StringLocalizer[ErrorMessages.ExceptionForcedDetail]
                );
            }

            if (string.IsNullOrEmpty(request.SearchTerm)) return new SampleQueryResponse(Samples);

            var itemFound = Samples.Where(s => s.Contains(request.SearchTerm, StringComparison.InvariantCulture)).ToList();

            if (itemFound.Count == 0) return new NotFound();

            return new SampleQueryResponse(itemFound);
        }
        catch (Exception exception)
        {
            Logger.LogCritical("There's an error on processing your request #### {Exception} ####", exception);

            return new Error(
                nameof(ErrorMessages.UnexpectedError),
                StringLocalizer[ErrorMessages.UnexpectedError],
                StringLocalizer[ErrorMessages.UnexpectedErrorDetail]
            );
        }
    }
}