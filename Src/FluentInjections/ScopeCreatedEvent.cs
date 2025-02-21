// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Events;

public record ScopeCreatedEvent(Guid ParentScopeId, Guid NewScopeId);
