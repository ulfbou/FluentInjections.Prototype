// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

namespace FluentInjections;

internal class WebApplication : IApplication<WebApplicationBuilder>
{
    private WebApplicationBuilder _builder;
    private Microsoft.AspNetCore.Builder.WebApplication _innerApplication;

    public WebApplication(WebApplicationBuilder builder, Microsoft.AspNetCore.Builder.WebApplication innerApplication)
    {
        Guard.NotNull(builder, nameof(builder));
        Guard.NotNull(innerApplication, nameof(innerApplication));
        _builder = builder;
        _innerApplication = innerApplication;
    }

    public WebApplicationBuilder Builder => _builder;

    public Task<TInnerApplication> GetInnerApplicationAsync<TInnerApplication>(CancellationToken? cancellationToken = null)
    {
        cancellationToken?.ThrowIfCancellationRequested();

        if (_innerApplication is TInnerApplication innerApplication)
        {
            return Task.FromResult(innerApplication);
        }

        throw new InvalidOperationException($"Inner application is not of type {typeof(TInnerApplication).FullName}");
    }

    public async Task RunAsync(CancellationToken? cancellationToken = null)
    {
        cancellationToken?.ThrowIfCancellationRequested();

        var innerApplication = await GetInnerApplicationAsync<Microsoft.AspNetCore.Builder.WebApplication>(cancellationToken);

        if (cancellationToken.HasValue)
        {
            cancellationToken.Value.Register(() => innerApplication.StopAsync());
        }

        await innerApplication.RunAsync();
    }

    public async Task StartAsync(CancellationToken? cancellationToken = null)
    {
        var innerApplication = await GetInnerApplicationAsync<Microsoft.AspNetCore.Builder.WebApplication>(cancellationToken);

        await innerApplication.StartAsync(cancellationToken ?? CancellationToken.None);
    }
    public Task StopAsync(CancellationToken? cancellationToken = null)
    {
        if (_innerApplication == null)
        {
            throw new InvalidOperationException("Application is not running.");
        }

        return _innerApplication.StopAsync(cancellationToken ?? CancellationToken.None);
    }
}
