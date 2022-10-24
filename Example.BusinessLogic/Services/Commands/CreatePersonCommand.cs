using Example.Domain.Entities;
using Example.Domain.Enums;

namespace Example.Application;

public record CreatePersonCommand : ICommand<CommandResultWithId>
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public PersonStatus? StatusStr { get; init; }
    public PersonStatus? StatusInt { get; init; }
    public PersonSex? Sex { get; init; }
    public PersonOccupation Occupation { get; init; }
    public PersonOccupationReason OccupationReason { get; init; }
    public PersonSex2? Sex2 { get; init; }
    public PersonOccupation2 Occupation2 { get; init; }
    public PersonOccupationReason2 OccupationReason2 { get; init; }
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
            FirstName = command.FirstName,
            LastName = command.LastName,
            StatusStr = command.StatusStr,
            StatusInt = command.StatusInt,
            Sex = command.Sex,
            Occupation = command.Occupation,
            OccupationReason = command.OccupationReason,
            Sex2 = command.Sex2,
            Occupation2 = command.Occupation2,
            OccupationReason2 = command.OccupationReason2
        };

        _dbContext.Person.Add(person);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CommandResultWithId(person.PersonId);
    }
}