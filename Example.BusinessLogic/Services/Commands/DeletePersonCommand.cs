using Example.DAL.Models;

namespace Example.Application
{
    public class DeletePersonCommand : ICommand, ICommandWithId<int>
    {
        public int Id { get; set;}
        public byte[] RowVersion { get; set; } = null!;
    }

    public class DeletePersonCommandHandler : ICommandHandler<DeletePersonCommand>
    {
        private readonly AppDbContext _dbContext;

        public DeletePersonCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResult> Handle(DeletePersonCommand command, CancellationToken cancellationToken = default)
        {
            Person? existing = await _dbContext.Person.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken);
            if (existing == null)
                throw new AppException($"Person with Id {command.Id} does not exist.");

            _dbContext.Entry(existing).Property(e => e.RowVersion).OriginalValue = command.RowVersion;

            _dbContext.Person.Remove(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CommandResult(-1); // TODO: what to return?
        }
    }
}