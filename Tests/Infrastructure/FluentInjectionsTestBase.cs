// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Extensions;
using FluentInjections.Internal;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Moq;

using System.Linq.Expressions;

using Xunit;

namespace FluentInjections.Tests.Infrastructure;

public abstract class FluentInjectionsTestBase : IAsyncLifetime
{
    protected IInjectionContainer Container { get; private set; } = null!;
    protected CancellationToken Cancellation { get; } = new();

    //public WebApplicationBuilderInjectionBuilder CreateInjectionBuilder(WebApplicationBuilder builder)
    //{
    //    var services = builder.Services;
    //    var provider = services.BuildServiceProvider();
    //    IServiceProviderFactory factory = new DefaultServiceProviderFactory();
    //    return new WebApplicationBuilderInjectionBuilder(provider, builder, factory, builder.Environment, builder.Configuration);
    //}
    ///// <summary>
    ///// Creates a new DI container configured using the provided action.
    ///// </summary>
    //protected async Task<IInjectionContainer> CreateContainerAsync(Action<IInjectionBuilder<WebApplicationBuilder>>? configure = null)
    //{
    //    var innerBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();
    //    var services = innerBuilder.Services;
    //    var provider = services.BuildServiceProvider();
    //    IServiceProviderFactory factory = new DefaultServiceProviderFactory();
    //    var builder = new WebApplicationBuilderInjectionBuilder(provider, builder, factory, innerBuilder.Environment, innerBuilder.Configuration);
    //    // Allow derived tests to add their own configuration
    //    ConfigureContainer(builder);
    //    // Apply test-specific configuration if provided
    //    configure?.Invoke(builder);
    //    Container = (IInjectionContainer)await builder.BuildAsync(Cancellation).ConfigureAwait(false);
    //    return Container;
    //}

    ///// <summary>
    ///// Registers a pre-created mock instance into the container.
    ///// </summary>
    //protected void RegisterMock<TService>(Mock<TService> mock, IInjectionContainer container) where TService : class
    //{
    //    container.RegisterInstance(mock.Object);
    //}

    ///// <summary>
    ///// Asserts that the service T can be resolved and, if a predicate is provided, that it meets the expected condition.
    ///// </summary>
    //protected void AssertResolved<T>(IInjectionContainer container, Expression<Func<T, bool>>? predicate = null)
    //{
    //    var service = container.Resolve<T>();
    //    Assert.NotNull(service);
    //    if (predicate != null)
    //    {
    //        var compiledPredicate = predicate.Compile();
    //        Assert.True(compiledPredicate(service), $"Resolved instance of {typeof(T).Name} did not match the expected condition.");
    //    }
    //}

    ///// <summary>
    ///// Virtual method to allow further configuration of the container.
    ///// </summary>
    //protected virtual void ConfigureContainer(IInjectionBuilder<WebApplicationBuilder> builder)
    //{
    //    // Default: No additional configuration.
    //    // Override in derived classes to add common test services.
    //}

    //// IAsyncLifetime implementation for proper asynchronous disposal.
    //public virtual async Task InitializeAsync()
    //{
    //    // Optionally perform async initialization tasks here.
    //    await Task.CompletedTask;
    //}

    public virtual async Task DisposeAsync()
    {
        try
        {
            //Container?.Dispose();
        }
        catch (Exception ex)
        {
            // Log or handle disposal exceptions as needed
            Console.WriteLine($"Exception during container disposal: {ex.Message}");
        }
        await Task.CompletedTask;
    }

    public abstract Task InitializeAsync();
}
