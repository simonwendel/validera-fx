// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ValideraFx.Web.ModelBinding;

internal abstract class ValueBinderBase(
    Type outerType,
    Type innerType,
    IModelMetadataProvider metadataProvider,
    IModelBinderFactory binderFactory) : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var innerMetadata = metadataProvider.GetMetadataForType(innerType);
        var binder = binderFactory.CreateBinder(new ModelBinderFactoryContext
        {
            Metadata = innerMetadata,
            BindingInfo = null,
            CacheToken = innerType
        });

        var nestedContext = DefaultModelBindingContext.CreateBindingContext(
            bindingContext.ActionContext,
            bindingContext.ValueProvider,
            innerMetadata,
            bindingInfo: null,
            modelName: bindingContext.ModelName);

        await binder.BindModelAsync(nestedContext);

        if (nestedContext.Result.IsModelSet)
        {
            var closedType = outerType.MakeGenericType(innerType);
            var name = bindingContext.ModelMetadata.ParameterName;
            var instance = Activator.CreateInstance(closedType, nestedContext.Result.Model, name);
            bindingContext.Result = ModelBindingResult.Success(instance);
        }
    }
}