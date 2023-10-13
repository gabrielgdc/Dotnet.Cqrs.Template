using System.Collections.Generic;
using Cqrs.Template.Domain.Exceptions;
using System.Threading.Tasks;
using Cqrs.Template.Dtos;
using Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Template.Controllers.V1;

[ApiVersion("1")]
[ApiController]
[Authorize(AuthenticationSchemes = CustomAuthenticationSchemes.Basic)]
public class InitialController : BaseController
{
    public InitialController(INotificationHandler<ExceptionNotification> notifications) : base(notifications)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetSample()
    {
        var ipsum = new List<string> { "Nothing", "Here", "Just", "Hello" };
        return Task.FromResult(CreateResponse(Ok(new Response<object>(ipsum))));
    }
}
