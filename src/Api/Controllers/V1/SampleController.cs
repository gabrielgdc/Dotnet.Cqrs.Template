using System.Threading.Tasks;
using Api.Dtos;
using Api.Factories;
using Application.Queries.SampleQuery;
using Application.Queries.SampleQuery.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

[ApiVersion("1")]
[ApiController]
public class SampleController(IMediator bus) : BaseController(bus)
{
    /// <summary>
    /// Sample get request endpoint
    /// </summary>
    /// <returns>Returns an string array with some sample text</returns>
    /// <response code="200">Returns an string array with some sample text</response>
    [HttpGet]
    [ProducesResponseType(typeof(Response<SampleQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] bool forceClientException)
    {
        var result = await Bus.Send(new SampleQuery(forceClientException));

        return result.Match(
            success => Ok(new Response<SampleQueryResponse>(success)),
            validationFailed => validationFailed.CreateProblemDetailsActionResult(HttpContext),
            error => error.CreateProblemDetailsActionResult(HttpContext)
        );
    }
}
