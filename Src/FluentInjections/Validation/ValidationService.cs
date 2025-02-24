// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel.DataAnnotations;

namespace FluentInjections.Validation;

internal class ValidationService
{
    internal Task<ValidationResult> ValidateServicesAsync(IServiceCollection services)
    {
        var context = new ValidationContext(services);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(services, context, results, true);

        if (!isValid)
        {
            var message = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            throw new ValidationException(message);
        }

        return Task.FromResult(ValidationResult.Success!);
    }
}
