using Application.Services;
using DAL.Models;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Services
{
    public class GetPersonQueryHandlerTests
    {
        [Fact]
        public async Task ExistingPerson_PersonReturned_Test()
        {
            using (var dbContext = TestDbContext.Create())
            {
                var person = new Person() { BusinessEntityId = 1, FirstName = "FirstName1", LastName = "LastName1", PersonType = "PersonType1" };
                dbContext.Person.Add(person);
                await dbContext.SaveChangesAsync();

                var handler = new GetPersonQueryHandler(dbContext);

                var res = await handler.ExecuteAsync(new GetPersonQuery(person.BusinessEntityId));

                Assert.NotNull(res);
                Assert.Equal(person.BusinessEntityId, res.BusinessEntityId);
            }
        }

        [Fact]
        public async Task NonExistingPerson_NullIsReturned_Test()
        {
            using (var dbContext = TestDbContext.Create())
            {
                var handler = new GetPersonQueryHandler(dbContext);

                var res = await handler.ExecuteAsync(new GetPersonQuery(1));

                Assert.Null(res);
            }
        }
    }
}
