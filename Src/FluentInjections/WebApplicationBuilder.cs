// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal class WebApplicationBuilder : IApplicationBuilder<WebApplicationBuilder>, IHostApplicationBuilder
{
    private readonly Microsoft.AspNetCore.Builder.WebApplicationBuilder _innerBuilder;

    public WebApplicationBuilder(Microsoft.AspNetCore.Builder.WebApplicationBuilder innerBuilder)
    {
        _innerBuilder = innerBuilder;
    }

    public IConfigurationManager Configuration => ((IHostApplicationBuilder)_innerBuilder).Configuration;

    public IHostEnvironment Environment => ((IHostApplicationBuilder)_innerBuilder).Environment;

    public ILoggingBuilder Logging => _innerBuilder.Logging;

    public IMetricsBuilder Metrics => _innerBuilder.Metrics;

    public IDictionary<object, object> Properties => ((IHostApplicationBuilder)_innerBuilder).Properties;

    public IServiceCollection Services => _innerBuilder.Services;

    public WebApplicationBuilder Builder => this;
    public TInnerBuilder GetInnerBuilder<TInnerBuilder>()
    {
        if (_innerBuilder is TInnerBuilder innerBuilder)
        {
            return innerBuilder;
        }

        throw new InvalidOperationException($"Inner builder is expecting type '{typeof(TInnerBuilder).FullName}', but the actual type is '{_innerBuilder.GetType().FullName}'.");
    }

    public Task<IApplication<WebApplicationBuilder>> BuildAsync(CancellationToken? cancellationToken = null)
    {
        var innerApplication = _innerBuilder.Build();
        return Task.FromResult<IApplication<WebApplicationBuilder>>(new WebApplication(this, innerApplication));
    }

    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
    {
        ((IHostApplicationBuilder)_innerBuilder).ConfigureContainer(factory, configure);
    }

    public void Dispose() => throw new NotImplementedException();
}
