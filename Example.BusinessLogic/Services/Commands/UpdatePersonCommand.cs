using Example.Domain.Entities;

namespace Example.Application;

public class UpdatePersonCommand : ICommand<CommandResult>, ICommandWithId<Guid>
{
    public Guid Id { get; set;}
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? Suffix { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public byte[] RowVersion { get; init; } = null!;
}

public class UpdatePersonCommandHandler : ICommandHandler<UpdatePersonCommand, CommandResult>
{
    private readonly AppDbContext _dbContext;

    public UpdatePersonCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CommandResult> Handle(UpdatePersonCommand command, CancellationToken cancellationToken = default)
    {
        Person? existing = await _dbContext.Person.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken);
        if (existing == null)
            throw new AppException($"Person with Id {command.Id} does not exist.");

        _dbContext.Entry(existing).Property(e => e.RowVersion).OriginalValue = command.RowVersion;

        existing.FirstName = command.FirstName;
        existing.LastName = command.LastName;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CommandResult();
    }
}