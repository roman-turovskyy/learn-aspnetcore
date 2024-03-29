﻿using Example.Application.Tests.Helpers;
using Example.Domain.Entities;
using Example.Domain.Enums;
using System.Threading.Tasks;

namespace Example.Application.Tests;

public class GetPersonQueryHandlerTests
{
    [Fact]
    public async Task ExistingPerson_PersonReturned_Test()
    {
        using (AppDbContext dbContext = TestDbContext.Create())
        {
            var person = new Person()
            {
                PersonId = Guid.NewGuid(),
                FirstName = "FirstName1",
                LastName = "LastName1",
                Occupation2 = PersonOccupation2.WorkingHard,
                Occupation22 = PersonOccupation2.WorkingHard,
                OccupationReason2 = PersonOccupationReason2.NobodyKnows
            };
            dbContext.Person.Add(person);
            await dbContext.SaveChangesAsync();

            GetPersonQueryHandler handler = new GetPersonQueryHandler(dbContext);

            Person? res = await handler.Handle(new GetPersonQuery(person.PersonId));

            Assert.NotNull(res);
            Assert.Equal(person.PersonId, res.PersonId);
        }
    }

    [Fact]
    public async Task NonExistingPerson_NullIsReturned_Test()
    {
        using (AppDbContext dbContext = TestDbContext.Create())
        {
            var handler = new GetPersonQueryHandler(dbContext);

            var res = await handler.Handle(new GetPersonQuery(Guid.NewGuid()));

            Assert.Null(res);
        }
    }
}
