// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

namespace FluentInjections.Adapters.AspNetCore
{
    public class ApplicationTypeMetadata : IApplicationTypeMetadata
    {
        public Type ApplicationType { get; }
        public Type BuilderType { get; }
        public string FrameworkName { get; }

        public ApplicationTypeMetadata(Type applicationType, Type builderType, string frameworkName)
        {
            ApplicationType = applicationType;
            BuilderType = builderType;
            FrameworkName = frameworkName;
        }
    }
}
