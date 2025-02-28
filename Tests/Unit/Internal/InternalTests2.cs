using FluentInjections.Configurators;
using FluentInjections.Internal;
using FluentInjections.Modules;
using FluentInjections.Services;
using FluentInjections.Tests.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using System.Reflection;

namespace FluentInjections.Tests.Units.Core.Internal
{
    //public partial class InternalTests2
    //{
    //    [Fact]
    //    public void InjectionBuilderFactory_CreateBuilder_CreatesInstance()
    //    {
    //        var services = new ServiceCollection();
    //        services.AddSingleton<ILoggerFactory, InternalLoggerFactory>();
    //        var factory = new InjectionBuilderFactory<MockBuilder>(services, null!);

    //        var builder = factory.CreateBuilder();

    //        Assert.NotNull(builder);
    //        Assert.IsType<InjectionBuilder<MockBuilder>>(builder);
    //    }

    //    [Fact]
    //    public void InjectionBuilderFactory_CreateBuilder_PassesArguments()
    //    {
    //        var services = new ServiceCollection();
    //        services.AddSingleton<ILoggerFactory, InternalLoggerFactory>();
    //        var arguments = new[] { "--test", "value" };
    //        var factory = new InjectionBuilderFactory<MockBuilder>(services, arguments);

    //        var builder = factory.CreateBuilder();

    //        Assert.NotNull(builder);
    //    }

    //    [Fact]
    //    public void InternalServicesComposer_CreateFactory_CreatesInstance()
    //    {
    //        var services = new ServiceCollection();
    //        var provider = services.BuildServiceProvider();
    //        var composer = new InternalServicesComposer<MockBuilder>(services, provider);

    //        var factory = composer.CreateFactory(null);

    //        Assert.NotNull(factory);
    //        Assert.IsType<InjectionBuilderFactory<MockBuilder>>(factory);
    //    }

    //    [Fact]
    //    public void InjectionBuilder_WithServices_ConfiguresAssemblies()
    //    {
    //        var builder = new InjectionBuilder<MockBuilder>(new InternalLoggerFactory());
    //        var configureCalled = false;

    //        builder.WithServices(assemblies => configureCalled = true);

    //        Assert.True(configureCalled);
    //    }

    //    [Fact]
    //    public void InjectionBuilder_WithServiceCollection_AddsServices()
    //    {
    //        var builder = new InjectionBuilder<MockBuilder>(new InternalLoggerFactory());
    //        var serviceCollection = new ServiceCollection();
    //        serviceCollection.AddSingleton<ITestService, TestService>();

    //        builder.WithServiceCollection(serviceCollection);

    //        var service = builder.Builder.Services.BuildServiceProvider().GetService<ITestService>();
    //        Assert.NotNull(service);
    //    }

    //    [Fact]
    //    public async Task InjectionBuilder_BuildAsync_RegistersModules()
    //    {
    //        var builder = new InjectionBuilder<MockBuilder>(new InternalLoggerFactory());
    //        var assembly = Assembly.GetExecutingAssembly();

    //        builder.WithServices(assemblies => assemblies.Add(assembly));

    //        var application = await builder.BuildAsync();

    //        var module = application.Provider.GetService<IModule>();
    //        Assert.NotNull(module);
    //    }

    //    [Fact]
    //    public async Task InjectionBuilder_BuildAsync_RegistersConfigurators()
    //    {
    //        var builder = new InjectionBuilder<MockBuilder>(new InternalLoggerFactory());
    //        var assembly = Assembly.GetExecutingAssembly();

    //        builder.WithServices(assemblies => assemblies.Add(assembly));

    //        var application = await builder.BuildAsync();

    //        var configurator = application.Provider.GetService<IConfigurator>();
    //        Assert.NotNull(configurator);
    //    }

    //    [Fact]
    //    public async Task InjectionBuilder_BuildAsync_RegistersConfigurableModules()
    //    {
    //        var builder = new InjectionBuilder<MockBuilder>(new InternalLoggerFactory());
    //        var assembly = Assembly.GetExecutingAssembly();

    //        builder.WithServices(assemblies => assemblies.Add(assembly));

    //        var application = await builder.BuildAsync();

    //        var configurableModule = application.Provider.GetService<IConfigurableModule<ITestConfigurator>>();
    //        Assert.NotNull(configurableModule);
    //    }

    //    [Fact]
    //    public void InternalLogger_IsEnabled_FiltersLogs()
    //    {
    //        var logger = new InternalLoggerFactory(LogLevel.Warning).CreateLogger("Test");

    //        Assert.False(logger.IsEnabled(LogLevel.Debug));
    //        Assert.True(logger.IsEnabled(LogLevel.Warning));
    //    }

    //    [Fact]
    //    public void InjectionBuilder_Dispose_DisposesResources()
    //    {
    //        var builder = new InjectionBuilder<MockBuilder>(new InternalLoggerFactory());
    //        builder.Dispose();

    //        //No Exceptions should be thrown.
    //    }

    //    public class TestModule : IModule
    //    {
    //        public int Priority => 0;
    //    }
    //    public class TestConfigurator : IConfigurator
    //    {
    //        public Task RegisterAsync(CancellationToken cancellationToken = default)
    //        {
    //            return Task.CompletedTask;
    //        }
    //    }
    //    public class TestConfigurableModule : IConfigurableModule<IServiceConfigurator>
    //    {
    //        public int Priority => 0;

    //        public Task ConfigureAsync(IServiceConfigurator configurator, CancellationToken cancellationToken = default)
    //        {
    //            return Task.CompletedTask;
    //        }
    //    }
    //    public interface ITestService { }
    //    public class TestService : ITestService { }

    //    public interface IMockBuilder<TBuilder> : IInjectionBuilder<TBuilder>
    //        where TBuilder : IInjectionBuilder<TBuilder>
    //    { }
    //}
}
