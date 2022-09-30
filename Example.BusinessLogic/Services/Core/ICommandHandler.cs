using MediatR;

namespace Example.Application;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CommandResult>
    where TCommand : IRequest<CommandResult>
{
}
