namespace Example.Application;

public record TestCommand : ICommand<CommandResultWithId>
{
    public Guid Id { get; init; }
}

public class TestCommandHandler : ICommandHandler<TestCommand, CommandResultWithId>
{
    public Task<CommandResultWithId> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CommandResultWithId(request.Id));
    }
}
