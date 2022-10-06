using Example.Common.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Common.Database;

public static class DatabaseExtensions
{
    public static DbContextOptionsBuilder<TContext> UseAudit<TContext>(this DbContextOptionsBuilder<TContext> builder, IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        return builder.UseAudit(serviceProvider.GetRequiredService<IMessageBus>());
    }

    public static DbContextOptionsBuilder<TContext> UseAudit<TContext>(this DbContextOptionsBuilder<TContext> builder, IMessageBus messageBus)
        where TContext : DbContext
    {
        builder.AddInterceptors(new AuditingInterceptor(messageBus));
        return builder;
    }
}
