using MediatR;

namespace Example.Application;

public interface ICommand : IRequest<CommandResult>
{
}
