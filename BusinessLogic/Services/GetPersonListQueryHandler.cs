﻿using Application.DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class GetPersonListQueryHandler : IQueryHandler<GetPersonListQuery, IList<Person>>
    {
        private readonly IAppDbContext _dbContext;

        public GetPersonListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Person>> ExecuteAsync(GetPersonListQuery query, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Person.AsNoTracking().Take(100).ToListAsync(cancellationToken);
        }
    }

    public class GetPersonListQuery
    {
    }
}
