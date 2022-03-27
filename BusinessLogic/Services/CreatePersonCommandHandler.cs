using Application.DAL;
using DAL.Models;

namespace Application.Services
{
    public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand>
    {
        private readonly AppDbContext _dbContext;

        public CreatePersonCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResult> ExecuteAsync(CreatePersonCommand command, CancellationToken cancellationToken = default)
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

    public class CreatePersonCommand
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
    }
}