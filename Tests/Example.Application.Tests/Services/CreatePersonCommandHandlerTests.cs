using System.Threading.Tasks;
using Xunit;

namespace Example.Application.Tests;

public class CreatePersonCommandHandlerTests
{
    [Fact]
    public async Task ValidCreatePersonCommand_PersonCreated_Test()
    {
        using (var dbContext = TestDbContext.Create())
        {
            var handler = new CreatePersonCommandHandler(dbContext);

            var res = await handler.Handle(new CreatePersonCommand
            {
                FirstName = "FirstName1",
                LastName = "LastName1"
            });

            var person = await dbContext.Person.FindAsync(res.NewEntityId);
            Assert.NotNull(person);
        }
    }
}
