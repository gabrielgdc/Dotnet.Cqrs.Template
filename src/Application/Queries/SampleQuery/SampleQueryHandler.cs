using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Queries.SampleQuery.Dtos;
using Infra.CrossCutting.Environments.Configurations;
using Microsoft.Extensions.Logging;

namespace Application.Queries.SampleQuery;

public class SampleQueryHandler(
    DatabaseConfiguration databaseConfiguration,
    ILogger logger)
    : QueryHandler<SampleQuery, SampleQueryResult>(databaseConfiguration,
        logger)
{
    private static readonly string[] Samples = ["Sample", "Query", "Execution"];

    public override async Task<SampleQueryResult> Handle(SampleQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!request.IsValid())
                return new ValidationFailed(request.ValidationResult.Errors);

            await Task.Yield();

            if (request.ForceClientException)
            {
                return
                    new Error(
                        nameof(SampleQueryErrorMessages.ExceptionForced),
                        SampleQueryErrorMessages.ExceptionForced,
                        ""
                    );
            }

            return new SampleQueryResponse(Samples);
        }
        catch (Exception exception)
        {
            Logger.LogCritical(
                "There's an error on processing your request #### {Exception} ####",
                exception);

            return new Error(
                nameof(SampleQueryErrorMessages.UnexpectedError),
                SampleQueryErrorMessages.UnexpectedError,
                ""
            );
        }
    }
}
