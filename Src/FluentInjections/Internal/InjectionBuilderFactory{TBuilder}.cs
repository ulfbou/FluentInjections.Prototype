// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Internal
{
    public class InjectionBuilderFactory<TBuilder>
        : IInjectionBuilderFactory<TBuilder>
            where TBuilder : IApplicationBuilder<TBuilder>
    {
        private readonly IServiceCollection _services;
        private readonly string[] _arguments;

        public InjectionBuilderFactory(IServiceCollection services, string[] arguments)
        {
            Guard.NotNull(services, nameof(services));
            _services = services;
            _arguments = arguments ?? Array.Empty<string>();
        }

        public IInjectionBuilder<TBuilder> CreateBuilder()
        {
            var loggerFactory = _services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            return (IInjectionBuilder<TBuilder>)Activator.CreateInstance(typeof(InjectionBuilder<>).MakeGenericType(typeof(TBuilder)), loggerFactory, _arguments)!;
        }
    }
}
