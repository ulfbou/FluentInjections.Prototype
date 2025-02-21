using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections
{
    public interface IInternalServicesComposer<TBuilder> where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        IServiceCollection Services { get; }
        IServiceProvider ServiceProvider { get; }

        IInjectionBuilderFactory<TBuilder> CreateFactory(string[]? arguments);

    }
}