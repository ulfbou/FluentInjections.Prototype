// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Interceptions;

public class InterceptorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public InterceptorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IInterceptor CreateInterceptor(Type interceptorType)
    {
        return (IInterceptor)_serviceProvider.GetRequiredService(interceptorType);
    }
}
