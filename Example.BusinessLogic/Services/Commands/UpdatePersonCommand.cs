using Example.Domain.Entities;
using Example.Domain.Enums;

namespace Example.Application;

public class UpdatePersonCommand : ICommand<CommandResult>, ICommandWithId<Guid>
{
    public Guid Id { get; set;}
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public PersonSex? Sex { get; init; }
    public PersonOccupation Occupation { get; init; }
    public PersonOccupationReason OccupationReason { get; init; }
    public PersonSex2? Sex2 { get; init; }
    public PersonOccupation2 Occupation2 { get; init; }
    public PersonOccupationReason2 OccupationReason2 { get; init; }
    public DateTime? ModifiedDate { get; init; }
    public byte[] RowVersion { get; init; }
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
        existing.Sex = command.Sex;
        existing.Occupation = command.Occupation;
        existing.OccupationReason = command.OccupationReason;
        existing.Sex2 = command.Sex2;
        existing.Occupation2 = command.Occupation2;
        existing.OccupationReason2 = command.OccupationReason2;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CommandResult();
    }
}