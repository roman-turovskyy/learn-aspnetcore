using Application.Services;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Services
{
    public class GetPersonListQueryServiceTests
    {
        [Fact]
        public async void EmptyDatabase_EmptyResult_Test()
        {
            using (var dbContext = TestDbContext.Create())
            {
                var service = new GetPersonListQueryService(dbContext);

                var res = await service.ExecuteAsync(new GetPersonListQuery());

                Assert.Empty(res);
            }
        }

        [Fact]
        public async void DataBaseWithSinglePerson_ThatPersonIsReturned_Test()
        {
            using (var dbContext = TestDbContext.Create())
            {
                var person = new Person() { BusinessEntityId = 1, FirstName = "FirstName1", LastName = "LastName1", PersonType = "PersonType1" };
                dbContext.Person.Add(person);
                await dbContext.SaveChangesAsync();

                var service = new GetPersonListQueryService(dbContext);

                var res = await service.ExecuteAsync(new GetPersonListQuery());

                Assert.Equal(1, res.Count);
                Assert.Equal(person.BusinessEntityId, res[0].BusinessEntityId);
            }
        }
    }
}
