using Example.Domain.Entities;

namespace Example.Application;

public class GetPersonLegacyQuery : IQuery<PersonLegacy?>
{
    public Guid Id { get; set; }

    public GetPersonLegacyQuery(Guid id)
    {
        Id = id;
    }
}

public class GetPersonLegacyQueryHandler : IQueryHandler<GetPersonLegacyQuery, PersonLegacy?>
{
    private readonly AppDbContext _dbContext;

    public GetPersonLegacyQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PersonLegacy?> Handle(GetPersonLegacyQuery query, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PersonLegacy.FindAsync(new object?[] { query.Id }, cancellationToken: cancellationToken);
    }
}
