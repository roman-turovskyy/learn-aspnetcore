using Example.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiEndpoint.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator) => _mediator = mediator;

    [HttpGet, Route("Query")]
    public async Task<string> GetSingle([FromQuery] TestQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpPost, Route("Command")]
    public async Task<Guid?> GetSingle(TestCommand command)
    {
        var res = await _mediator.Send(command);
        return res.NewEntityId;
    }
}
