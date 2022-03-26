namespace Application.Services
{
    public interface IQueryService<TQuery, TResult>
    {
        Task<TResult> ExecuteAsync(TQuery query);
    }
}
