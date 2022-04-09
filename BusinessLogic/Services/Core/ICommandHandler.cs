using MediatR;

namespace Application.Services
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CommandResult>
        where TCommand : IRequest<CommandResult>
    {
    }
}
