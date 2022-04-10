using MediatR;

namespace Example.Application
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IRequest<TResult>
    {
    }
}
