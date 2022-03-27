using Application.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IQueryHandler<GetPersonListQuery, IList<Person>> _getPersonListQuery;
        private readonly ICommandHandler<CreatePersonCommandHandler> _createPersonCommand;

        public PersonController(
            IQueryHandler<GetPersonListQuery, IList<Person>> personQueryService,
            ICommandHandler<CreatePersonCommandHandler> createPersonCommand)
        {
            _getPersonListQuery = personQueryService;
            _createPersonCommand = createPersonCommand;
        }

        [HttpGet]
        public async Task<ICollection<Person>> Get()
        {
            return await _getPersonListQuery.ExecuteAsync(new GetPersonListQuery());
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreatePersonCommandHandler createPerson)
        {
            await _createPersonCommand.ExecuteAsync(createPerson);
            return Ok();
        }
    }
}