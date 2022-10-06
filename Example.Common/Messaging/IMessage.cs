namespace Example.Common.Messaging;

public interface IMessage
{
    string MessageName { get; }
    string MessageId => Guid.NewGuid().ToString();
}
