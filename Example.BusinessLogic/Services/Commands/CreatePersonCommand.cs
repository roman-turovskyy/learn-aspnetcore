﻿using Example.DAL.Models;

namespace Example.Application;

public record CreatePersonCommand : ICommand<CommandResultWithId>
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string? Suffix { get; init; }
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
            BusinessEntity = new BusinessEntity {
                Rowguid = Guid.NewGuid()
            },
            // TODO: validation
            FirstName = command.FirstName,
            LastName = command.LastName,
            Suffix = command.Suffix,
            PersonType = "EM",
            RowVersion = Array.Empty<byte>() // otherwise InMemoryDatabase fails during testing
        };
        _dbContext.Person.Add(person);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CommandResultWithId(person.BusinessEntityId);
    }
}