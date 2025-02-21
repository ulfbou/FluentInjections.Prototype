using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Events;

public static class EventsExtensions
{
    public static void AddEventHandler(this IServiceCollection services)
    {
        services.AddTransient<IConcurrentEventHandlerRegistry, ConcurrentEventHandlerRegistry>();
        services.AddTransient<ConcurrentEventHandlerInvoker>();
        services.AddTransient<IConcurrentEventBus, ConcurrentEventBus>();
    }
}
