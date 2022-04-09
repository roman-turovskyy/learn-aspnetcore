using MediatR;

namespace Application.Services
{
    public interface ICommand : IRequest<CommandResult>
    {
    }
}
