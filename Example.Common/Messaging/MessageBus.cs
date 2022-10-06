namespace Example.Common.Messaging;

public class MessageBus : IMessageBus
{
    public Task SendAsync(IMessage message)
    {
        Console.WriteLine($"Sending message {message}.");
        return Task.CompletedTask;
    }
}
