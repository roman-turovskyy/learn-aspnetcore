using Application.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IQueryService<GetPersonListQuery, IList<Person>> _personQueryService;

        public PersonController(IQueryService<GetPersonListQuery, IList<Person>> personQueryService)
        {
            _personQueryService = personQueryService;
        }

        [HttpGet]
        public async Task<ICollection<Person>> Get()
        {
            return await _personQueryService.ExecuteAsync(new GetPersonListQuery());
        }
    }
}