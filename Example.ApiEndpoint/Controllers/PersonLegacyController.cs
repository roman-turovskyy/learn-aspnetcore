using MediatR;
using Microsoft.AspNetCore.Mvc;
using Example.Domain.Entities;
using Example.Application;
using Microsoft.AspNetCore.OData.Query;

namespace Example.ApiEndpoint.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonLegacyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _appDbContext;

    public PersonLegacyController(IMediator mediator, AppDbContext appDbContext)
    {
        _mediator = mediator;
        _appDbContext = appDbContext;
    }

    [HttpGet, Route("odata"), EnableQuery]
    public IQueryable<PersonLegacy> GetListOdata()
    {
        return _appDbContext.PersonLegacy;
    }

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