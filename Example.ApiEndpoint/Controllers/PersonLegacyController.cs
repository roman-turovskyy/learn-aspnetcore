using MediatR;
using Microsoft.AspNetCore.Mvc;
using Example.Domain.Entities;

namespace Example.ApiEndpoint.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonLegacyController : ControllerBase
{
    private readonly IMediator _mediator;

    public PersonLegacyController(IMediator mediator) => _mediator = mediator;

    [HttpGet, Route("{id}")]
    public async Task<ActionResult<Person>> GetSingle(Guid id)
    {
        var res = await _mediator.Send(new GetPersonQuery(id));
        if (res == null)
            return NotFound($"Person {id} does not exist.");

        return res;
    }

    [HttpPost]
    public async Task<CommandResult> Create(CreatePersonLegacyCommand cmd)
    {
        return await _mediator.Send(cmd);
    }
}