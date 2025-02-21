using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections
{
    internal sealed class WebApplicationBuilderInjectionBuilderFactory : InjectionBuilderFactoryBase<WebApplicationBuilder>, IInjectionBuilderFactory<WebApplicationBuilder>
    {
        private readonly string[] _arguments;

        public WebApplicationBuilderInjectionBuilderFactory(
            string[] arguments,
            IServiceCollection services,
            IInternalServicesComposer<WebApplicationBuilder> composer)
            : base(services, composer)
        {
            _arguments = arguments;
        }

        public override IInjectionBuilder<WebApplicationBuilder> CreateBuilder()
        {
            var sp = _composer.ServiceProvider;
            var innerBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(_arguments);
            var builder = new WebApplicationBuilder(innerBuilder);
            var factory = sp.GetRequiredService<IServiceProviderFactory>();
            return new WebApplicationBuilderInjectionBuilder(_composer.ServiceProvider, builder, factory);
        }
    }
}
