using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections;

internal abstract class InjectionBuilderFactoryBase<TBuilder> : IInjectionBuilderFactory<TBuilder> where TBuilder : class, IApplicationBuilder<TBuilder>
{
    protected readonly IServiceCollection _services;
    protected readonly IInternalServicesComposer<TBuilder> _composer;

    public InjectionBuilderFactoryBase(IServiceCollection services, IInternalServicesComposer<TBuilder> composer)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _composer = composer ?? throw new ArgumentNullException(nameof(composer));
    }

    public abstract IInjectionBuilder<TBuilder> CreateBuilder();
}
