// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

//using FluentInjections.Abstractions;
//using FluentInjections.Abstractions.Adapters;
//using FluentInjections.Logging;

//using Microsoft.AspNetCore.Builder;

//using ILoggerFactoryProvider = FluentInjections.Logging.ILoggerFactoryProvider;

//namespace FluentInjections.Adapters.AspNetCore
//{
//    public class WebApplicationAdapter : IConcreteApplicationAdapter<WebApplication>
//    {
//        private readonly WebApplication _innerApplication;
//        private readonly WebApplicationBuilderAdapter _builderAdapter; // Store the builder adapter for relationship (optional, depending on needs)
//        private readonly ILoggerFactoryProvider _loggerProvider;

//        public WebApplicationAdapter(WebApplication application, WebApplicationBuilderAdapter builderAdapter, ILoggerFactoryProvider loggerProvider)
//        {
//            _innerApplication = application ?? throw new ArgumentNullException(nameof(application));
//            _builderAdapter = builderAdapter ?? throw new ArgumentNullException(nameof(builderAdapter)); // Optional
//            _loggerProvider = loggerProvider ?? NullLoggerFactoryProvider.Instance;
//        }

//        public WebApplication ConcreteApplication => _innerApplication;
//        public IBuilderAdapter<WebApplication> Adapter => _builderAdapter;
//        public ILoggerFactoryProvider LoggerFactoryProvider => _loggerProvider;

//        public async Task RunAsync()
//        {
//            await _innerApplication.RunAsync();
//        }

//        public Task RunAsync(CancellationToken? cancellationToken = null) => throw new NotImplementedException();
//        public Task StopAsync(CancellationToken? cancellationToken = null) => throw new NotImplementedException();
//    }
//}
