// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections
{
    internal class ModuleManagerHandler
    {
        private IModuleManager moduleManager;

        public ModuleManagerHandler(IModuleManager moduleManager) => this.moduleManager = moduleManager;
    }
}