namespace Example.Application;

public record CommandResult;

public record CommandResultWithId(int NewEntityId) : CommandResult;
