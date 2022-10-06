﻿using Example.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.Application;

public class GetPersonListQuery : IQuery<IList<Person>>
{
}

public class GetPersonListQueryHandler : IQueryHandler<GetPersonListQuery, IList<Person>>
{
    private readonly AppDbContext _dbContext;

    public GetPersonListQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Person>> Handle(GetPersonListQuery request, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Person.AsNoTracking().Take(100).ToListAsync(cancellationToken);
    }
}
