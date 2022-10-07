using Example.Domain.Entities;

namespace Example.Application;

public record CreatePersonCommand : ICommand<CommandResultWithId>
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}

public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand, CommandResultWithId>
{
    private readonly AppDbContext _dbContext;

    public CreatePersonCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CommandResultWithId> Handle(CreatePersonCommand command, CancellationToken cancellationToken = default)
    {
        var person = new Person
        {
            // TODO: validation
            FirstName = command.FirstName,
            LastName = command.LastName
        };
        _dbContext.Person.Add(person);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CommandResultWithId(person.PersonId);
    }
}