using DAL.Models;

namespace Example.Application
{
    public class CreatePersonCommand : ICommand
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
    }

    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly AppDbContext _dbContext;

        public CreatePersonCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResult> Handle(CreatePersonCommand command, CancellationToken cancellationToken = default)
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
                PersonType = "EM"
            };
            _dbContext.Person.Add(person);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new CommandResult(person.BusinessEntityId);
        }
    }
}