namespace Application.Services.Commands
{
    public class TestCommand : ICommand
    {
        public int Id { get; set; }
    }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task<CommandResult> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CommandResult(request.Id));
        }
    }
}
