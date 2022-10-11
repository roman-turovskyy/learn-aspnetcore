using Example.Domain.Entities;

namespace Example.Application;

public record CreatePersonLegacyCommand : ICommand<CommandResultWithId>
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
}

public class CreatePersonLegacyCommandHandler : ICommandHandler<CreatePersonLegacyCommand, CommandResultWithId>
{
    private readonly AppDbContext _dbContext;

    public CreatePersonLegacyCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CommandResultWithId> Handle(CreatePersonLegacyCommand command, CancellationToken cancellationToken = default)
    {
        var person = new PersonLegacy
        {
            // TODO: validation
            FirstName = command.FirstName,
            LastName = command.LastName
        };
        _dbContext.PersonLegacy.Add(person);

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CommandResultWithId(person.PersonLegacyId);
    }
}