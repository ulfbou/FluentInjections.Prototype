using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections
{
    internal class WebApplicationBuilder : IApplicationBuilder<WebApplicationBuilder>
    {
        public WebApplicationBuilder(Microsoft.AspNetCore.Builder.WebApplicationBuilder innerBuilder) => InnerBuilder = innerBuilder;

        public IServiceCollection Services => throw new NotImplementedException();

        public WebApplicationBuilder Builder => throw new NotImplementedException();

        public Microsoft.AspNetCore.Builder.WebApplicationBuilder InnerBuilder { get; }

        public Task<IApplication<WebApplicationBuilder>> BuildAsync(CancellationToken? cancellationToken = null) => throw new NotImplementedException();
        public void Dispose() => throw new NotImplementedException();
    }
}