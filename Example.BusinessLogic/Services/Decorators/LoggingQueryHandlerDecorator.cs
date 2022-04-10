namespace Example.Application
{
    public class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private IQueryHandler<TQuery, TResult> _decorated;

        public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated)
        {
            _decorated = decorated;
        }

        public async Task<TResult> Handle(TQuery query, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Before invoking query {query.GetType()}");
            var res = await _decorated.Handle(query, cancellationToken);
            Console.WriteLine($"After invoking query {query.GetType()}");
            return res;
        }
    }
}
