namespace Example.Common.Messaging;

public interface IMessageBus
{
    Task SendAsync(IMessage message);
}
