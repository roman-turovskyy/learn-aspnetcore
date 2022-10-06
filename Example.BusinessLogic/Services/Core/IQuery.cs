using MediatR;

namespace Example.Application;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
