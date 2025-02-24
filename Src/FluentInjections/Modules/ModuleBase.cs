// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Modules
{
    public class ModuleBase : IModule
    {
        public virtual int Priority => 0;
    }
}
