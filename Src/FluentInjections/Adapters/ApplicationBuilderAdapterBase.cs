// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Adapters
{
    public abstract class ApplicationBuilderAdapterBase<TBuiltApplication, TConcreteBuilder> : IApplicationBuilderAdapter<TBuiltApplication>
    {
        protected TConcreteBuilder _innerBuilder;

        protected ApplicationBuilderAdapterBase(TConcreteBuilder innerBuilder)
        {
            Guard.NotNull(innerBuilder, nameof(innerBuilder));
            _innerBuilder = innerBuilder;
        }

        public abstract Task<IApplicationAdapter<TBuiltApplication>> BuildAsync(CancellationToken cancellationToken = default);
        public object InnerBuilder => _innerBuilder!;
        public virtual void ConfigureServices(IServiceCollection services) { }
        public virtual void ConfigureApplication(TBuiltApplication application) { }
    }
}
