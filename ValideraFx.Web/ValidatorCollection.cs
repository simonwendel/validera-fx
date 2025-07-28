// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.Extensions.DependencyInjection;
using ValideraFx.Core;

namespace ValideraFx.Web;

public class ValidatorCollection : IValidatorCollection
{
    private readonly IServiceCollection services;
    private Dictionary<Type, Type>? validators = null;

    public ValidatorCollection(IServiceCollection services, bool eagerLoad)
    {
        ArgumentNullException.ThrowIfNull(services);
        this.services = services;
        if (eagerLoad)
        {
            LoadValidators();
        }
    }

    public object? GetValidatorFor(Type type, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(serviceProvider);

        if (validators is null)
        {
            LoadValidators();
        }

        if (validators!.TryGetValue(type, out var validatorType))
        {
            return serviceProvider.GetService(validatorType);
        }

        throw new InvalidOperationException($"No suitable validator for type {type.FullName} found.");
    }

    private void LoadValidators()
    {
        validators = new Dictionary<Type, Type>();
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
}