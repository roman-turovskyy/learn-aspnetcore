using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Application
{
    public class GetPersonListQuery : IQuery<IList<Person>>
    {
    }

    public class GetPersonListQueryHandler : IQueryHandler<GetPersonListQuery, IList<Person>>
    {
        private readonly IAppDbContext _dbContext;

        public GetPersonListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Person>> Handle(GetPersonListQuery request, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Person.AsNoTracking().Take(100).ToListAsync(cancellationToken);
        }
    }
}
