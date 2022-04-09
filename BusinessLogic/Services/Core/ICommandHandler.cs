namespace Application.Services
{
    public interface ICommandHandler<in TCommand>
    {
        Task<CommandResult> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
