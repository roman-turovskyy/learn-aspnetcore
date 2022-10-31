using Example.Application.Tests.Helpers;
using Example.Domain.Enums;

namespace Example.Application.Tests;

public class CreatePersonCommandHandlerTests
{
    [Fact]
    public async Task ValidCreatePersonCommand_PersonCreated_Test()
    {
        using (AppDbContext dbContext = TestDbContext.Create())
        {
            var handler = new CreatePersonCommandHandler(dbContext);

            var res = await handler.Handle(new CreatePersonCommand
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                Occupation2 = PersonOccupation2.WorkingHard,
                OccupationReason2 = PersonOccupationReason2.NobodyKnows
            });

            var person = await dbContext.Person.FindAsync(res.NewEntityId);
            Assert.NotNull(person);
        }
    }
}
