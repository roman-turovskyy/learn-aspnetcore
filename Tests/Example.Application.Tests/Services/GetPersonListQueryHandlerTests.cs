using Example.Domain.Entities;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Example.Application.Tests;

public class GetPersonListQueryHandlerTests
{
    [Fact]
    public async Task EmptyDatabase_EmptyResult_Test()
    {
        using (var dbContext = TestDbContext.Create())
        {
            var handler = new GetPersonListQueryHandler(dbContext);

            var res = await handler.Handle(new GetPersonListQuery());

            Assert.Empty(res);
        }
    }

    [Fact]
    public async Task DataBaseWithSinglePerson_ThatPersonIsReturned_Test()
    {
        using (var dbContext = TestDbContext.Create())
        {
            var person = new Person() { PersonId = Guid.NewGuid(), FirstName = "FirstName1", LastName = "LastName1", RowVersion = Array.Empty<byte>() };
            dbContext.Person.Add(person);
            await dbContext.SaveChangesAsync();

            var handler = new GetPersonListQueryHandler(dbContext);

            var res = await handler.Handle(new GetPersonListQuery());

            Assert.Equal(1, res.Count);
            Assert.Equal(person.PersonId, res[0].PersonId);
        }
    }
}
