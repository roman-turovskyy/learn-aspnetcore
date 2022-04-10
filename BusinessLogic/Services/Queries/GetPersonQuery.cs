using DAL.Models;

namespace Example.Application
{
    public class GetPersonQuery : IQuery<Person?>
    {
        public int Id { get; set; }

        public GetPersonQuery(int id)
        {
            Id = id;
        }
    }

    public class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, Person?>
    {
        private readonly AppDbContext _dbContext;

        public GetPersonQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person?> Handle(GetPersonQuery query, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Person.FindAsync(new object?[] { query.Id }, cancellationToken: cancellationToken);
        }
    }
}
