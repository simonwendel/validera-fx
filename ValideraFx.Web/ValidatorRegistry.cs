// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.Extensions.DependencyInjection;
using ValideraFx.Core;

namespace ValideraFx.Web;

public class ValidatorRegistry : IValidatorRegistry
{
    private readonly IServiceCollection services;
    private Dictionary<Type, Type>? validators = null;

    public ValidatorRegistry(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        this.services = services;
    }

    public object? GetValidatorFor(Type type, IServiceProvider serviceProvider, ValidationOptions options)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(options);

        if (validators is null)
        {
            Load();
        }

        if (!validators!.TryGetValue(type, out var validatorType))
        {
            throw new InvalidOperationException($"No suitable validator for type {type.FullName} found.");
        }

        var validator = serviceProvider.GetService(validatorType);
        if (options.DontRenderValues)
        {
            var prop = validatorType.GetProperty("RenderValue");
            if (prop is not null && prop.CanWrite)
            {
                prop.SetValue(validator, false);
            }
        }

        return validator;

    }

    public void Load()
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