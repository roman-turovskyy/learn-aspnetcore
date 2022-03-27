namespace Application.Services
{
    public interface ICommandHandler<TCommand>
    {
        Task<CommandResult> ExecuteAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
