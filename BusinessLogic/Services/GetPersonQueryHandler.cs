using Application.DAL;
using DAL.Models;

namespace Application.Services
{
    public class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, Person?>
    {
        private readonly AppDbContext _dbContext;

        public GetPersonQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person?> ExecuteAsync(GetPersonQuery query, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Person.FindAsync(new object?[] { query.Id }, cancellationToken: cancellationToken);
        }
    }

    public class GetPersonQuery
    {
        public int Id { get; set; }

        public GetPersonQuery(int id)
        {
            Id = id;
        }
    }
}
