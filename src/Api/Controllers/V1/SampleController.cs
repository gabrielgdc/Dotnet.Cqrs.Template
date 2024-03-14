using System.Threading.Tasks;
using Api.Dtos;
using Api.Factories;
using Application.Queries.SampleQuery;
using Application.Queries.SampleQuery.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

/// <summary>
/// Represents an API controller handling sample queries related to the application.
/// </summary>
/// <remarks>
/// This controller inherits from the <see cref="BaseController"/> class, providing common functionality and setup for API controllers.
/// It exposes a GET endpoint to execute a sample query and return the results or appropriate error responses.
/// </remarks>
[ApiVersion("1")]
[ApiController]
public class SampleController(IMediator bus, ICustomProblemDetailsFactory customProblemDetailsFactory) : BaseController(bus, customProblemDetailsFactory)
{
    /// <summary>
    /// Handles a GET request to execute a sample query.
    /// </summary>
    /// <param name="forceClientException">An optional boolean query parameter indicating whether to force a client-side exception.</param>
    /// <returns>A Response&lt;SampleQueryResponse&gt; object containing the successful query results or a ProblemDetails object with detailed error information.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Response<SampleQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] bool forceClientException)
    {
        var result = await Bus.Send(new SampleQuery(forceClientException));

        return result.Match(
            success => Ok(new Response<SampleQueryResponse>(success)),
            validationFailed => CustomProblemDetailsFactory.CreateProblemDetailsActionResult(validationFailed),
            error => CustomProblemDetailsFactory.CreateProblemDetailsActionResult(error)
        );
    }
}
