// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using ValideraFx.Core;

namespace ValideraFx.Web;

public class UntrustedValueBinderProvider : IModelBinderProvider
{
    private readonly ConcurrentDictionary<Type, IModelBinder> cache = new();

    public IModelBinder? GetBinder(ModelBinderProviderContext? context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var modelType = context.Metadata.ModelType;
        if (!modelType.IsGenericType || modelType.GetGenericTypeDefinition() != typeof(UntrustedValue<>))
        {
            return null;
        }

        var innerType = modelType.GetGenericArguments()[0];

        var metadataProvider = context.Services.GetRequiredService<IModelMetadataProvider>();
        var binderFactory = context.Services.GetRequiredService<IModelBinderFactory>();

        var modelBinder = new UntrustedValueBinder(innerType, metadataProvider, binderFactory);
        return cache.GetOrAdd(modelType, _ => modelBinder);
    }
}