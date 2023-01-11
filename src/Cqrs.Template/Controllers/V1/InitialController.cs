using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cqrs.Template.Domain.Exceptions;
using Cqrs.Template.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Template.Controllers.V1;

[ApiVersion("1")]
[ApiController]
public class InitialController : BaseController
{
    public InitialController(INotificationHandler<ExceptionNotification> notifications) : base(notifications)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public Task<IActionResult> Sample()
    {
        throw new Exception("dsaads");
        var ipsum = new List<string> { "Nothing", "Here", "Just", "Hello" };
        return Task.FromResult(CreateResponse(Ok(new Response<object>(ipsum))));
    }
}
