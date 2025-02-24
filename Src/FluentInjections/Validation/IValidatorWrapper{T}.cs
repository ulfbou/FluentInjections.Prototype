// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace FluentInjections.Validation
{
    public interface IValidatorWrapper<T>
    {
        ValidationResult Validate(T instance);
        T ValidateAndThrow(T instance);
    }
}
