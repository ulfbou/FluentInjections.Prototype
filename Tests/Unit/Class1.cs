using FluentInjections.Injection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Tests.Units;

public class InjectionBuilderTests
{
    [Fact]
    public void For_ShouldReturnInjectionBuilderInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        var innerBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();
        var webApplicationBuilder = new WebApplicationBuilder(innerBuilder);

        // Act
        var injectionBuilder = InjectionBuilder.For<WebApplicationBuilder>(null, services);

        // Assert
        Assert.NotNull(injectionBuilder);
        Assert.IsAssignableFrom<IInjectionBuilder<WebApplicationBuilder>>(injectionBuilder);
    }

    [Fact]
    public void For_ShouldConfigureServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddLogging();
        var innerBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();
        var webApplicationBuilder = new WebApplicationBuilder(innerBuilder);

        // Act
        var injectionBuilder = InjectionBuilder.For<WebApplicationBuilder>(null, services);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var logger = serviceProvider.GetService<ILogger<WebApplicationBuilder>>();
        Assert.NotNull(logger);
    }

    [Fact]
    public void For_ShouldThrowException_WhenServicesIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => InjectionBuilder.For<WebApplicationBuilder>(null, null));
    }
}
