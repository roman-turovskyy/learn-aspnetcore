using MediatR;

namespace Example.Application;

public interface ICommand<out TResult> : IRequest<TResult>
{
}