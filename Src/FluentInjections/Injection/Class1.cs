//// Copyright (c) FluentInjections Project. All rights reserved.
//// Licensed under the MIT License. See LICENSE in the project root for license information.

//using FluentInjections.Application;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace FluentInjections.Injection
//{
//    public class ApplicationComposer
//    {
//        private readonly ILoggerFactory _loggerFactory;

//        public ApplicationComposer(ILoggerFactory loggerFactory)
//        {
//            _loggerFactory = loggerFactory;
//        }

//        public IInjectionBuilder<TBuilder> CreateInjectionBuilder<TBuilder>(TBuilder builder)
//            where TBuilder : class, IApplicationBuilder<TBuilder>
//        {
//            var logger = _loggerFactory.CreateLogger<InjectionBuilder<TBuilder>>();
//            return new InjectionBuilder<TBuilder>(builder, _loggerFactory);
//        }
//    }
//}
