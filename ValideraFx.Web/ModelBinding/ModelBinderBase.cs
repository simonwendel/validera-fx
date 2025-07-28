// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal abstract class ModelBinderBase(
    Type innerType,
    IModelMetadataProvider metadataProvider,
    IModelBinderFactory binderFactory) : IModelBinder
{
    protected Type InnerType { get; } = innerType;

    protected abstract Type GetTypeToBind();

    protected abstract object?[] GetConstructorArguments(
        ModelBindingContext bindingContext,
        ModelBindingContext nestedContext);

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var innerMetadata = metadataProvider.GetMetadataForType(InnerType);
        var binder = binderFactory.CreateBinder(new ModelBinderFactoryContext
        {
            Metadata = innerMetadata,
            BindingInfo = null,
            CacheToken = InnerType
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
            var typeToBind = GetTypeToBind();
            var constructorArguments = GetConstructorArguments(bindingContext, nestedContext);
            try
            {
                var instance = Activator.CreateInstance(typeToBind, constructorArguments);
                bindingContext.Result = ModelBindingResult.Success(instance);
            }
            catch (TargetInvocationException exception)
                when (exception.InnerException is ValidationException validationException)
            {
                bindingContext.ModelState.AddModelError(validationException.ValidatedName,
                    validationException.ValidationMessage);
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }
    }
}