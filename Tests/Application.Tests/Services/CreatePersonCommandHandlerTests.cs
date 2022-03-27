using Application.Services;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Services
{
    public class CreatePersonCommandHandlerTests
    {
        [Fact]
        public async Task ValidCreatePersonCommand_PersonCreated_Test()
        {
            using (var dbContext = TestDbContext.Create())
            {
                var handler = new CreatePersonCommandHandler(dbContext);

                var res = await handler.ExecuteAsync(new CreatePersonCommand{
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Suffix = "Suffix1"
                });                
                
                Assert.NotNull(res.NewEntityId);
            }
        }
    }
}
