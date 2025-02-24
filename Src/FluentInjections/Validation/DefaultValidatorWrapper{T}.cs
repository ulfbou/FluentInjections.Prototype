// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace FluentInjections.Validation
{
    public class DefaultValidatorWrapper<T> : IValidatorWrapper<T>
    {
        private readonly IServiceProvider _provider;

        public DefaultValidatorWrapper(IServiceProvider provider)
        {
            Guard.NotNull(provider, nameof(provider));
            _provider = provider;
        }

        public ValidationResult Validate(T instance)
        {
            Guard.NotNull(instance, nameof(instance));
            var context = new ValidationContext(instance!, _provider, null);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(instance!, context, results, true))
            {
                return ValidationResult.Success ?? new ValidationResult("Validation succeeded.");
            }

            return new ValidationResult(string.Join("\n", results));
        }

        public T ValidateAndThrow(T instance)
        {
            var result = Validate(instance);

            if (result != ValidationResult.Success)
            {
                throw new ValidationException(result.ErrorMessage);
            }

            return instance;
        }
    }
}
