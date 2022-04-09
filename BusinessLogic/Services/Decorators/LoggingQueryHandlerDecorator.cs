namespace Application.Services.Decorators
{
    public class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    {
        private IQueryHandler<TQuery, TResult> _decorated;

        public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated)
        {
            _decorated = decorated;
        }

        public async Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Before invoking query {query.GetType()}");
            var res = await _decorated.ExecuteAsync(query, cancellationToken);
            Console.WriteLine($"After invoking query {query.GetType()}");
            return res;
        }
    }
}
