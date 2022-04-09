using MediatR;

namespace Application.Services
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
