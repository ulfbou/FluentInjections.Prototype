﻿// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface IConfigurator
{
    Task RegisterAsync(CancellationToken? cancellationToken = null);
}
