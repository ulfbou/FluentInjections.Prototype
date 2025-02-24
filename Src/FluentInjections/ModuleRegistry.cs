//// Copyright (c) FluentInjections Project. All rights reserved.
//// Licensed under the MIT License. See LICENSE in the project root for license information.

//using FluentInjections.Validation;
//using FluentInjections;

//using Microsoft.Extensions.DependencyInjection;
//using System.Collections.Concurrent;
//using FluentInjections.Extensions;

//internal class ModuleRegistry : IModuleRegistry
//{
//    private readonly IServiceCollection _services;
//    private readonly ConcurrentDictionary<Type, List<IModule>> _modules = new();

//    public ModuleRegistry(IServiceCollection services)
//    {
//        _services = services ?? throw new ArgumentNullException(nameof(services));
//    }

//    public async Task ApplyAsync<TConfigurator>(TConfigurator configurator, CancellationToken? cancellationToken = default)
//        where TConfigurator : IConfigurator
//    {
//        Guard.NotNull(configurator, nameof(configurator));

//        var exceptions = new List<Exception>();

//        foreach (var moduleList in _modules.Values)
//        {
//            foreach (var module in moduleList.ToList()) // Iterate over a copy for thread safety
//            {
//                if (module is IConfigurableModule<TConfigurator> configurableModule)
//                {
//                    try
//                    {
//                        await configurableModule.ConfigureAsync(configurator, cancellationToken);
//                    }
//                    catch (Exception ex)
//                    {
//                        exceptions.Add(new AggregateException($"Failed to configure module of type {module.GetType().Name}.", ex));
//                    }
//                }
//            }
//        }

//        if (exceptions.Any())
//        {
//            throw new AggregateException("One or more modules failed to apply configuration.", exceptions);
//        }
//    }

//    public Task RegisterAsync(Type configuratorType, IModule moduleInstance, CancellationToken? cancellationToken = null)
//    {
//        Guard.NotNull(configuratorType, nameof(configuratorType));
//        Guard.NotNull(moduleInstance, nameof(moduleInstance));

//        if (!moduleInstance.GetType().ImplementsGenericInterface(typeof(IConfigurableModule<>), configuratorType))
//        {
//            throw new InvalidOperationException($"Module of type {moduleInstance.GetType().Name} does not implement IConfigurableModule<TConfigurator>.");
//        }

//        _modules.AddOrUpdate(
//            configuratorType,
//            _ => new List<IModule> { moduleInstance },
//            (_, existing) =>
//            {
//                if (existing.Any(m => m.GetType() == moduleInstance.GetType()))
//                {
//                    throw new InvalidOperationException($"Module of type {moduleInstance.GetType().Name} is already registered.");
//                }

//                existing.Add(moduleInstance);
//                return existing;
//            });

//        return Task.CompletedTask;
//    }

//    public async Task RegisterAsync<TConfigurator>(IConfigurableModule<TConfigurator> module, CancellationToken? cancellationToken = default)
//        where TConfigurator : IConfigurator
//    {
//        Guard.NotNull(module, nameof(module));

//        var moduleType = module.GetType();

//        _modules.AddOrUpdate(
//            typeof(TConfigurator),
//            _ => new List<IModule> { module },
//            (_, existing) =>
//            {
//                if (existing.Any(m => m.GetType() == moduleType))
//                {
//                    throw new InvalidOperationException($"Module of type {moduleType.Name} is already registered.");
//                }
//                existing.Add(module);
//                return existing;
//            });

//        if (module is IValidatable validatableModule)
//        {
//            await validatableModule.ValidateAsync(cancellationToken);
//        }
//    }

//    public Task RegisterAsync<TModule, TConfigurator>(TModule module, CancellationToken? cancellationToken = default)
//        where TModule : IConfigurableModule<TConfigurator>
//        where TConfigurator : IConfigurator
//        => RegisterAsync((IConfigurableModule<TConfigurator>)module, cancellationToken);

//    public async Task RegisterAsync<TModule, TConfigurator>(Func<TModule> factory, Action<TModule> configure, CancellationToken? cancellationToken = default)
//        where TModule : IConfigurableModule<TConfigurator>
//        where TConfigurator : IConfigurator
//    {
//        Guard.NotNull(factory, nameof(factory));
//        Guard.NotNull(configure, nameof(configure));

//        var module = factory();
//        configure(module);
//        await RegisterAsync((IConfigurableModule<TConfigurator>)module, cancellationToken);
//    }

//    public Task UnregisterAsync<TModule, TConfigurator>(TModule module, CancellationToken? cancellationToken = default)
//        where TModule : IConfigurableModule<TConfigurator>
//        where TConfigurator : IConfigurator
//    {
//        Guard.NotNull(module, nameof(module));

//        if (_modules.TryGetValue(typeof(TConfigurator), out var moduleList))
//        {
//            lock (moduleList)  // Thread safety
//            {
//                moduleList.Remove(module); // Use object equality

//                if (moduleList.Count == 0)
//                {
//                    _modules.TryRemove(typeof(TConfigurator), out _);
//                }
//            }
//        }

//        return Task.CompletedTask;
//    }

//    public async Task InitializeAsync(CancellationToken? cancellationToken = default)
//    {
//        var exceptions = new List<Exception>();

//        foreach (var moduleList in _modules.Values)
//        {
//            foreach (var module in moduleList.ToList().OfType<IInitializable>())
//            {
//                try
//                {
//                    await module.InitializeAsync(cancellationToken);
//                }
//                catch (Exception ex)
//                {
//                    exceptions.Add(new AggregateException($"Failed to initialize module of type {module.GetType().Name}.", ex));
//                }
//            }
//        }

//        if (exceptions.Any())
//        {
//            throw new AggregateException("One or more modules failed to initialize.", exceptions);
//        }
//    }

//    public Task UnregisterAsync<TConfigurator>(Type moduleType, IConfigurableModule<TConfigurator> module, CancellationToken? cancellationToken = default)
//        where TConfigurator : IConfigurator
//    {
//        Guard.NotNull(moduleType, nameof(moduleType));
//        Guard.NotNull(module, nameof(module));

//        var configuratorType = typeof(TConfigurator);

//        if (_modules.TryGetValue(configuratorType, out var moduleList))
//        {
//            lock (moduleList)
//            {
//                moduleList.Remove(module);

//                if (moduleList.Count == 0)
//                {
//                    _modules.TryRemove(configuratorType, out _);
//                }
//            }
//        }

//        return Task.CompletedTask;
//    }

//    internal IEnumerable<IConfigurableModule<TConfigurator>> GetModules<TConfigurator>()
//        where TConfigurator : IConfigurator
//    {
//        return _modules.Values.SelectMany(list => list.OfType<IConfigurableModule<TConfigurator>>());
//    }
//}
