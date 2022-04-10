namespace Application.Services.Queries
{
    public class TestQuery : IQuery<string>
    {
        public string Message { get; set; } = "";
    }

    public class TestQueryHandler : IQueryHandler<TestQuery, string>
    {
        public Task<string> Handle(TestQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Test message: " + request.Message);
        }
    }
}
