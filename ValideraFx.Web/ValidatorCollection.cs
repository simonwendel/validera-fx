// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.Extensions.DependencyInjection;
using ValideraFx.Core;

namespace ValideraFx.Web;

public class ValidatorCollection : IValidatorCollection
{
    private readonly Dictionary<Type, Type> validators = new();

    public ValidatorCollection(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        foreach (var descriptor in services)
        {
            if (!descriptor.ServiceType.IsGenericType ||
                descriptor.ServiceType.GetGenericTypeDefinition() != typeof(IValidator<>))
            {
                continue;
            }

            var innerType = descriptor.ServiceType.GetGenericArguments()[0];
            validators[innerType] = descriptor.ServiceType;
        }
    }

    public object? GetValidatorFor(Type type, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        if (validators.TryGetValue(type, out var validatorType))
        {
            return serviceProvider.GetService(validatorType);
        }

        throw new InvalidOperationException($"No suitable validator for type {type.FullName} found.");
    }
}