using FluentInjections.Adapters.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using FluentInjections.Tests.Utils; // Assuming TestWebApplicationBuilder, IWebApplicationBuilder are in this namespace
using FluentInjections.Tests.Units;
using FluentInjections.Abstractions.Adapters; // Assuming TestWebApplication, IWebApplication are in this namespace

namespace FluentInjections.Tests.Units.Middleware
{
    public class MiddlewareTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public MiddlewareTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task WebApplication_UseMiddleware_RegistersAndExecutesMiddleware_ConcreteWebApp_VerifyUse() // Test name updated - Pragmatic Verify Use
        {
            // 1. Arrange (Set up with Concrete WebApplication and Mock IApplicationBuilder for VERIFICATION)
            var mockIApplicationBuilder = new Mock<IApplicationBuilder>(); // Mock IApplicationBuilder for VERIFICATION ONLY

            var webAppBuilder = WebApplication.CreateBuilder(); // Use CONCRETE WebApplicationBuilder
            var webApplication = webAppBuilder.Build(); // Use CONCRETE WebApplication

            // **Important:**  Replace the *concrete* WebApplication's IApplicationBuilder with our mock for verification purposes.
            // **Caution:** This is using reflection to access an internal service provider and replace the IApplicationBuilder.
            // This is brittle and for demonstration in a test context ONLY.  Avoid in production code.
            var serviceProvider = webApplication.Services;
            var appBuilderField = typeof(WebApplication).GetField("_appBuilder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (appBuilderField != null)
            {
                appBuilderField.SetValue(webApplication, mockIApplicationBuilder.Object); // Replace concrete IApplicationBuilder with Mock
            }
            else
            {
                throw new InvalidOperationException("Could not find _appBuilder field in WebApplication using reflection. Test setup might be broken due to ASP.NET Core changes.");
            }


            var webAppBuilderAdapter = new CoreWebApplicationBuilderAdapter(webAppBuilder); // Pass CONCRETE WebApplicationBuilder
            var appBuilder = new DefaultApplicationBuilder<WebApplicationBuilder, WebApplication, CoreWebApplicationBuilderAdapter, CoreWebApplicationAdapter>(webAppBuilderAdapter); // Generics: WebApplicationBuilder, WebApplication - CONCRETE TYPES

            bool useMiddlewareCalled = false; // Flag for IApplicationBuilder.Use verification

            appBuilder.RegisterApplicationAdapterFactory<CoreWebApplicationAdapter>((concreteApp, builderAdapter) =>
            {
                // Mock IApplicationBuilder.Use for VERIFICATION - Set up callback for verification
                mockIApplicationBuilder.Setup(ab => ab.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                    .Callback<Func<RequestDelegate, RequestDelegate>>(middleware =>
                    {
                        useMiddlewareCalled = true; // Set flag when IApplicationBuilder.Use is called on MOCKED IApplicationBuilder
                    });

                return new CoreWebApplicationAdapter(webApplication, builderAdapter); // Pass CONCRETE WebApplication
            });


            IApplicationAdapter<WebApplication, CoreWebApplicationAdapter> app = await appBuilder.BuildAsync(); // IApplication with WebApplication and CoreWebApplicationAdapter - CONCRETE TYPES

            bool loggingMiddlewareExecuted = false;
            bool helloMiddlewareExecuted = false;
            string responseBody = "";


            app.Adapter.RegisterMiddleware(
                next =>
                {
                    return async context =>
                    {
                        loggingMiddlewareExecuted = true;
                        var loggerFactory = app.LoggerFactory;
                        var logger = loggerFactory.CreateLogger<MiddlewareTests>();
                        logger.LogInformation("LoggingMiddleware executed");
                        _outputHelper.WriteLine("LoggingMiddleware executed");
                        await next(context);
                    };
                });


            app.Adapter.RegisterMiddleware(next =>
            {
                return async context =>
                {
                    helloMiddlewareExecuted = true;
                    responseBody = "Hello, World from Concrete WebApp Middleware - Verified Use!"; // Updated message
                    await Task.CompletedTask; // Or context.Response.WriteAsync if you want to test response, but for now just verifying Use call
                };
            });


            await app.RunAsync(); // RunAsync - Concrete WebApplication's RunAsync


            Assert.True(useMiddlewareCalled, "IApplicationBuilder.Use should have been called."); // Assert IApplicationBuilder.Use was CALLED on MOCKED INSTANCE
            Assert.True(loggingMiddlewareExecuted, "LoggingMiddleware execution flag should be true."); // Verify Middleware 1 flag
            Assert.True(helloMiddlewareExecuted, "HelloMiddleware execution flag should be true."); // Verify Middleware 2 flag
            Assert.Equal("", responseBody.Trim()); // Assert empty response body for now
        }
    }
}
