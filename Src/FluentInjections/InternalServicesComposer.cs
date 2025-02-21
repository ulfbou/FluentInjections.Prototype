using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections;

public class InternalServicesComposer<TBuilder> : IInternalServicesComposer<TBuilder> where TBuilder : class, IApplicationBuilder<TBuilder>
{
    private readonly string[] _arguments;
    private readonly IServiceCollection _services;
    private readonly IServiceProvider _serviceProvider;

    public IServiceCollection Services => _services;
    public IServiceProvider ServiceProvider => _serviceProvider;

    public InternalServicesComposer(string[]? arguments, IServiceCollection? services = null)
    {
        _arguments = arguments ?? Array.Empty<string>();
        _services = services ?? new ServiceCollection();
        _serviceProvider = BuildeServiceProvider();
    }

    public IInjectionBuilderFactory<TBuilder> CreateFactory(string[]? arguments)
    {
        return _serviceProvider.GetRequiredService<IInjectionBuilderFactory<TBuilder>>();
    }

    private IServiceProvider BuildeServiceProvider()
    {
        _services.AddSingleton<IServiceCollection>(_services);
        _services.AddSingleton<IInjectionBuilderFactory<TBuilder>, DefaultInjectionBuilderFactory<TBuilder>>();
        _services.AddSingleton<IInternalServicesComposer<TBuilder>>(
            sp => new InternalServicesComposer<TBuilder>(_arguments, _services));
        _services.AddSingleton<IInjectionBuilderFactory<WebApplicationBuilder>>(
            sp =>
            {
                var innerBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(_arguments);
                var builder = new WebApplicationBuilder(innerBuilder);
                var composer = sp.GetRequiredService<IInternalServicesComposer<WebApplicationBuilder>>();
                return new WebApplicationBuilderInjectionBuilderFactory(_arguments, _services, composer);
            });

        return _services.BuildServiceProvider();
    }
}
