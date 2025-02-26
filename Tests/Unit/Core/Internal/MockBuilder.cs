using FluentInjections.Application;
using FluentInjections.Collections;
using FluentInjections.Internal;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using static FluentInjections.Tests.Units.Core.Internal.InternalTests2;

namespace FluentInjections.Tests.Units.Core.Internal
{
    internal class MockBuilder : FluentApplication<MockBuilder>, IApplicationBuilder<MockBuilder>
    {
        public MockBuilder(MockBuilder builder, IHost host) : base(builder, host)
        {
        }

        public IServiceCollection Services { get; } = new ServiceCollection();

        public IServiceProvider ServiceProvider { get; private set; }

        public MockBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            configureServices(Services);
            ServiceProvider = Services.BuildServiceProvider();
            return this;
        }

        public MockBuilder UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware
        {
            Services.AddSingleton<TMiddleware>();
            return this;
        }

        public IHost BuildHost()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    foreach (var service in Services)
                    {
                        services.Add(service);
                    }
                })
                .Build();

            return host;
        }

        public TInnerBuilder GetInnerBuilder<TInnerBuilder>() => throw new NotImplementedException();
        public MockBuilder AddConfigurationSource(IConfigurationSource configurationSource) => throw new NotImplementedException();
        public Task<IApplication<MockBuilder>> BuildAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public void Dispose() => throw new NotImplementedException();
    }
}
