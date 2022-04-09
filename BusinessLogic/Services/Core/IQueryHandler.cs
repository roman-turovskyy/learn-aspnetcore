namespace Application.Services
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
