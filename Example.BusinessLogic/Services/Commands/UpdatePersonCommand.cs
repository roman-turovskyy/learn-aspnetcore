using Example.DAL.Models;
using Microsoft.VisualBasic;

namespace Example.Application
{
    public class UpdatePersonCommand : ICommand
    {
        public int Id { get; set;}
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Suffix { get; set; }
        public byte[] RowVersion { get; set; } = null!;
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

            existing.FirstName = command.FirstName;
            existing.LastName = command.LastName;
            existing.Suffix = command.Suffix;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CommandResult(existing.BusinessEntityId);
        }
    }
}