// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.Extensions.Hosting;

namespace FluentInjections.Adapters.AspNetCore
{
    //public class CoreHostApplicationAdapter
    //{
    //    private readonly IHost _innerHost;
    //    private readonly CoreHostBuilderAdapter _builderAdapter;

    //    public CoreHostApplicationAdapter(IHost host, CoreHostBuilderAdapter builderAdapter)
    //    {
    //        _innerHost = host ?? throw new ArgumentNullException(nameof(host));
    //        _builderAdapter = builderAdapter ?? throw new ArgumentNullException(nameof(builderAdapter));
    //    }

    //    public async Task RunAsync(CancellationToken? cancellationToken = null)
    //    {
    //        if (cancellationToken.HasValue)
    //        {
    //            await _innerHost.RunAsync(cancellationToken.Value);
    //        }
    //        else
    //        {
    //            await _innerHost.RunAsync();
    //        }
    //    }

    //    public async Task StopAsync(CancellationToken? cancellationToken = null)
    //    {
    //        if (cancellationToken.HasValue)
    //        {
    //            await _innerHost.StopAsync(cancellationToken.Value);
    //        }
    //        else
    //        {
    //            await _innerHost.StopAsync();
    //        }
    //    }
    //}
}
