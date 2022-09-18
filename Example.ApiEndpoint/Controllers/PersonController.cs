using Example.DAL.Models;
using Example.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonController(IMediator mediator) => _mediator = mediator;

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<Person>> GetSingle(int id)
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

        [HttpPost, Route("{id}")]
        public async Task<CommandResult> Create(CreatePersonCommand createPerson)
        {
            return await _mediator.Send(createPerson);
        }

        [HttpPut, Route("{id}")]
        public async Task<CommandResult> Update([FromRoute]int id, [FromBody]UpdatePersonCommand updatePerson)
        {
            return await _mediator.Send(updatePerson);
        }
    }
}