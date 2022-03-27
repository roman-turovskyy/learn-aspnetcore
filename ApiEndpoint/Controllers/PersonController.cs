using Application.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IQueryHandler<GetPersonQuery, Person> _getPersonQuery;
        private readonly IQueryHandler<GetPersonListQuery, IList<Person>> _getPersonListQuery;
        private readonly ICommandHandler<CreatePersonCommand> _createPersonCommand;

        public PersonController(
            IQueryHandler<GetPersonQuery, Person> getPersonQuery,
            IQueryHandler<GetPersonListQuery, IList<Person>> personQueryService,
            ICommandHandler<CreatePersonCommand> createPersonCommand)
        {
            _getPersonQuery = getPersonQuery;
            _getPersonListQuery = personQueryService;
            _createPersonCommand = createPersonCommand;
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<Person>> GetSingle(int id)
        {
            var res = await _getPersonQuery.ExecuteAsync(new GetPersonQuery(id));
            if (res == null)
                return NotFound($"Person {id} does not exist.");
            return res;
        }

        [HttpGet]
        public async Task<ICollection<Person>> GetList()
        {
            return await _getPersonListQuery.ExecuteAsync(new GetPersonListQuery());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreatePersonCommand createPerson)
        {
            await _createPersonCommand.ExecuteAsync(createPerson);
            return Ok();
        }
    }
}