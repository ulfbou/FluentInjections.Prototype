// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Injection;

// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace FluentInjections
{
    internal class ApplicationComposer
    {
        private ILoggerFactory loggerFactory;

        public ApplicationComposer(ILoggerFactory loggerFactory) => this.loggerFactory = loggerFactory;

        internal IInjectionBuilder<TBuilder> CreateInjectionBuilder<TBuilder>(TBuilder builder)
            where TBuilder : IApplicationBuilder<TBuilder> => throw new NotImplementedException();
    }
}