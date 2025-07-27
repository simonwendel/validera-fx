// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal class ModelBinderProvider : IModelBinderProvider
{
    private readonly ConcurrentDictionary<Type, IModelBinder> cache = new();

    public IModelBinder? GetBinder(ModelBinderProviderContext? context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var metadataProvider = context.Services.GetRequiredService<IModelMetadataProvider>();
        var binderFactory = context.Services.GetRequiredService<IModelBinderFactory>();

        var modelType = context.Metadata.ModelType;
        if (cache.TryGetValue(modelType, out var cachedBinder))
        {
            return cachedBinder;
        }

        switch (modelType.IsGenericType)
        {
            case true when modelType.GetGenericTypeDefinition() == typeof(UntrustedValue<>):
            {
                var innerType = modelType.GetGenericArguments()[0];
                var modelBinder = new UntrustedModelBinder(innerType, metadataProvider, binderFactory);
                return cache.GetOrAdd(modelType, _ => modelBinder);
            }
            case true when modelType.GetGenericTypeDefinition() == typeof(TrustedValue<>):
            {
                var innerType = modelType.GetGenericArguments()[0];
                var modelBinder = new TrustedModelBinder(innerType, metadataProvider, binderFactory);
                return cache.GetOrAdd(modelType, _ => modelBinder);
            }
            default:
                return null;
        }
    }
}