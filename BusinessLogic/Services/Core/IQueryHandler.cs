namespace Application.Services
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
