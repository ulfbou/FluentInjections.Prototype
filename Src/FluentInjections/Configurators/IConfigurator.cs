// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentInjections.Configurators
{
    public interface IConfigurator
    {
        Task RegisterAsync(CancellationToken cancellationToken = default);
    }
}

