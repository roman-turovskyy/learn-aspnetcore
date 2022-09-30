namespace Example.Application;

public record TestCommand : ICommand
{
    public int Id { get; init; }
}

public class TestCommandHandler : ICommandHandler<TestCommand>
{
    public Task<CommandResult> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CommandResult(request.Id));
    }
}
