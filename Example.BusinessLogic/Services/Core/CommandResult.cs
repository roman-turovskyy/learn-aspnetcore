namespace Example.Application;

public record CommandResult;

public record CommandResultWithId(Guid NewEntityId) : CommandResult;
