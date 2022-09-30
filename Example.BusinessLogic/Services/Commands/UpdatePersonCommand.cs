using Example.DAL.Models;

namespace Example.Application;

public class UpdatePersonCommand : ICommand, ICommandWithId<int>
{
    public int Id { get; set;}
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? Suffix { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public byte[] RowVersion { get; init; } = null!;
}

public class UpdatePersonCommandHandler : ICommandHandler<UpdatePersonCommand>
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
        //_dbContext.Entry(existing).Property(e => e.ModifiedDate).OriginalValue = command.ModifiedDate;
        _dbContext.Entry(existing).Property(e => e.ModifiedDate).OriginalValue = DateTime.Parse("2022-08-18T13:55:11.887");

        existing.FirstName = command.FirstName;
        existing.LastName = command.LastName;
        existing.Suffix = command.Suffix;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CommandResult(existing.BusinessEntityId);
    }
}