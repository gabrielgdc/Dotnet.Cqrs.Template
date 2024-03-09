using Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]/v{version:apiVersion}")]
[ServiceFilter(typeof(GlobalExceptionFilterAttribute))]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public abstract class BaseController(IMediator bus) : Controller
{
    protected readonly IMediator Bus = bus;
}
