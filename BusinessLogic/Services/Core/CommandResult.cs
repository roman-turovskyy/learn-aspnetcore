namespace Example.Application
{
    public struct CommandResult
    {
        public int? NewEntityId { get; private set; }

        public CommandResult(int newEntityId)
        {
            NewEntityId = newEntityId;
        }
    }
}
