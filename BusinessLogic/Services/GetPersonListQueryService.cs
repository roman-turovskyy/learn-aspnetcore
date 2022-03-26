using Application.DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class GetPersonListQueryService : IQueryService<GetPersonListQuery, IList<Person>>
    {
        private readonly AppDbContext _dbContext;

        public GetPersonListQueryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Person>> ExecuteAsync(GetPersonListQuery query)
        {
            return await _dbContext.Person.Take(100).AsNoTracking().ToListAsync();
        }
    }

    public class GetPersonListQuery : QueryBase
    {
    }
}
