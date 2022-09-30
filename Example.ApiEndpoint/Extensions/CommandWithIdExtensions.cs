using Example.Application;

namespace Example.ApiEndpoint.Extensions;

public static class CommandWithIdExtensions
{
    public static void FetchIdFromRoute<T>(this ICommandWithId<T> cmd, T idFromRoute)
    {
        if (idFromRoute == null || idFromRoute.Equals(default(T)))
            throw new BadHttpRequestException($"idFromRoute is required.");

        bool modelHasIdValue = cmd.Id != null && !cmd.Id.Equals(default(T));
        if (modelHasIdValue && !idFromRoute.Equals(cmd.Id))
            throw new BadHttpRequestException($"cmd.Id != route/Id ('{cmd.Id}' != '{idFromRoute}').");

        cmd.Id = idFromRoute;
    }
}
