using MediatR;
using Microsoft.AspNetCore.Mvc;
using Example.ApiEndpoint.Extensions;
using Example.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ApiEndpoint.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public PersonController(IMediator mediator, AppDbContext appDbContext, IMapper mapper)
    {
        _mediator = mediator;
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    [HttpGet, Route("{id}")]
    public async Task<ActionResult<Person>> GetSingle(Guid id)
    {
        var res = await _mediator.Send(new GetPersonQuery(id));
        if (res == null)
            return NotFound($"Person {id} does not exist.");

        return res;
    }

    [HttpGet]
    public async Task<ICollection<Person>> GetList()
    {
        return await _mediator.Send(new GetPersonListQuery());
    }

    [HttpGet, Route("odata"), EnableQuery]
    public IQueryable<Person> GetListOdata()
    {
        return _appDbContext.Person;
        //return _appDbContext.Person.ProjectTo<PersonDTO>(_mapper.ConfigurationProvider);
    }

    [HttpPost]
    public async Task<CommandResult> Create(CreatePersonCommand cmd)
    {
        return await _mediator.Send(cmd);
    }

    [HttpPut, Route("{id}")]
    public async Task<CommandResult> Update(Guid id, UpdatePersonCommand cmd)
    {
        cmd.FetchIdFromRoute(id);

        return await _mediator.Send(cmd);
    }

    [HttpDelete, Route("{id}")]
    public async Task<CommandResult> Delete(Guid id, DeletePersonCommand cmd)
    {
        cmd.FetchIdFromRoute(id);

        return await _mediator.Send(cmd);
    }
}