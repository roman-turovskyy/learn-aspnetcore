namespace Example.Application
{
    public interface ICommandWithId<T>
    {
        T Id { get; set; }
    }
}
