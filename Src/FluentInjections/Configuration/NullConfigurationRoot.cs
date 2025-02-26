// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace FluentInjections.Configuration
{
    public interface IConfigurationProvider
    {
        IConfiguration GetConfiguration();
    }
}
