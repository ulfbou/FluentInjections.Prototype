
namespace FluentInjections
{
    internal class WebApplicationBuilderInjectionBuilder : InjectionBuilderBase<WebApplicationBuilder>
    {
        public WebApplicationBuilderInjectionBuilder(
            IServiceProvider internalServiceProvider,
            WebApplicationBuilder builder,
            IServiceProviderFactory serviceProviderFactory) : base(internalServiceProvider, builder, serviceProviderFactory)
        { }

        public override Task<IApplication<WebApplicationBuilder>> BuildAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task RegisterExternalModulesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}