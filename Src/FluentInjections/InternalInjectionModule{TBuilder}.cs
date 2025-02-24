//// Copyright (c) FluentInjections Project. All rights reserved.
//// Licensed under the MIT License. See LICENSE in the project root for license information.

//using FluentInjections.Application;
//using FluentInjections.DependencyInjection;
//using FluentInjections.Events;
//using FluentInjections.Lifecycle;
//using FluentInjections.Middlewares;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace FluentInjections;

//public interface InternalModule<TBuilder> where TBuilder : IApplicationBuilder<TBuilder>
//{
//    Task RegisterAsync(IServiceCollection services, CancellationToken cancellationToken);
//}
//internal class InternalServiceInjectionModule<TBuilder>
//    : Module<IServiceConfigurator, IApplicationBuilder<TBuilder>>
//    , IConfigurableModule<IServiceConfigurator>
//    where TBuilder : IApplicationBuilder<TBuilder>
//{
//    public InternalServiceInjectionModule(IApplicationBuilder<TBuilder> builder) : base(builder) { }

//    public override int Priority => 0;

//    public override Task ConfigureAsync(IServiceConfigurator configurator, CancellationToken? cancellationToken = null)
//    {
//        // Register MODULE-SPECIFIC internal services here
//        configurator.Register<ILifecycleConfigurator>(nameof(LifecycleConfigurator)).UsingFactory(
//            (sp, cancellation) => new ValueTask<ILifecycleConfigurator>(new LifecycleConfigurator(sp.GetRequiredService<IComponentRegistry<ILifecycleComponent>>(), sp.GetRequiredService<ILoggerFactory>())));

//        configurator.Register<IMiddlewareConfigurator<TBuilder>>(nameof(MiddlewareConfigurator<TBuilder>))
//                    .UsingFactory((sp, cancellation) => new ValueTask<IMiddlewareConfigurator<TBuilder>>(
//                        new MiddlewareConfigurator<TBuilder>(
//                            sp.GetRequiredService<IApplication<TBuilder>>(),
//                            sp.GetRequiredService<IComponentRegistry<IMiddlewareComponent>>(),
//                            sp.GetRequiredService<ILoggerFactory>())));

//        configurator.Register<IServiceConfigurator>(nameof(ServiceConfigurator))
//                    .UsingFactory((sp, cancellation) => new ValueTask<IServiceConfigurator>(
//                        new ServiceConfigurator(
//                            sp.GetRequiredService<IServiceCollection>(),
//                            sp.GetRequiredService<IComponentRegistry<IServiceComponent>>(),
//                            sp.GetRequiredService<ILoggerFactory>())));

//        configurator.Register<IConcurrentEventBus>(nameof(Events.ConcurrentEventBus))
//                    .UsingFactory((sp, cancellation) => new ValueTask<IConcurrentEventBus>(
//                        new ConcurrentEventBus(
//                            sp.GetRequiredService<IConcurrentEventHandlerRegistry>(),
//                            sp.GetRequiredService<IConcurrentEventHandlerInvoker>(),
//                            sp.GetRequiredService<ILogger<ConcurrentEventBus>>())));

//        configurator.Register<IConcurrentEventHandlerInvoker>(nameof(ConcurrentEventHandlerInvoker))
//                    .UsingFactory((sp, cancellation) => new ValueTask<IConcurrentEventHandlerInvoker>(
//                        new ConcurrentEventHandlerInvoker(
//                            sp.GetRequiredService<ILogger<ConcurrentEventHandlerInvoker>>())));

//        configurator.Register<IConcurrentEventHandlerRegistry>(nameof(ConcurrentEventHandlerRegistry))
//                    .UsingFactory((sp, cancellation) => new ValueTask<IConcurrentEventHandlerRegistry>(
//                        new ConcurrentEventHandlerRegistry(
//                            sp.GetRequiredService<ILogger<ConcurrentEventHandlerRegistry>>(),
//                            TimeSpan.FromSeconds(30))));

//        configurator.Register<IApplication<WebApplicationBuilder>>(nameof(WebApplication))
//                    .UsingFactory((sp, cancellation) =>
//                    {
//                        WebApplicationBuilder builder = sp.GetRequiredService<WebApplicationBuilder>();
//                        var innerBuilder = builder.GetInnerBuilder<Microsoft.AspNetCore.Builder.WebApplicationBuilder>();
//                        var innerApplication = innerBuilder.Build();
//                        return new ValueTask<IApplication<WebApplicationBuilder>>(
//                            new WebApplication(
//                                builder,
//                                innerApplication));
//                    });

//        configurator.Register<ModuleManagerHandler>(nameof(ModuleManagerHandler))
//                    .UsingFactory((sp, cancellation) => new ValueTask<ModuleManagerHandler>(
//                        new ModuleManagerHandler(
//                            sp.GetRequiredService<IModuleManager>())));

//        RegisterComponentServices<ILifecycleComponent>(configurator);
//        RegisterComponentServices<IMiddlewareComponent>(configurator);
//        RegisterComponentServices<IServiceComponent>(configurator);

//        return Task.CompletedTask;
//    }

//    private void RegisterComponentServices<TComponent>(IServiceConfigurator configurator)
//    where TComponent : IComponent
//    {
//        configurator.Register<IComponentResolver<TComponent>>(nameof(ComponentResolver<TComponent>))
//                    .UsingFactory((sp, cancellation) => new ValueTask<IComponentResolver<TComponent>>(
//                        new ComponentResolver<TComponent>(
//                            sp.GetRequiredService<IComponentRegistry<TComponent>>(),
//                            sp)));

//        configurator.Register<IComponentRegistry<TComponent>>(nameof(ComponentRegistry<TComponent>))
//                    .UsingFactory((sp, cancellation) => new ValueTask<IComponentRegistry<TComponent>>(
//                        new ComponentRegistry<TComponent>(
//                            sp.GetRequiredService<ILogger<ComponentRegistry<TComponent>>>())));
//    }
//}
