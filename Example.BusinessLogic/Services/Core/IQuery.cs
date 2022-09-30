using MediatR;

namespace Example.Application;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
