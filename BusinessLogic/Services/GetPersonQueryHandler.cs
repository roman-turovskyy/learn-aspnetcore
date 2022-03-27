using Application.DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, Person>
    {
        private readonly AppDbContext _dbContext;

        public GetPersonQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person> ExecuteAsync(GetPersonQuery query, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Person.FindAsync(query.Id);
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
