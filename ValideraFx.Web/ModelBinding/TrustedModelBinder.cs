// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal class TrustedModelBinder(
    Type innerType,
    IModelMetadataProvider metadataProvider,
    IModelBinderFactory binderFactory)
    : ModelBinderBase(innerType, metadataProvider, binderFactory)
{
    protected override Type GetTypeToBind() => typeof(TrustedValue<>).MakeGenericType(InnerType);

    protected override object?[] GetConstructorArguments(
        ModelBindingContext bindingContext,
        ModelBindingContext nestedContext)
    {
        var services = bindingContext.HttpContext.RequestServices;
        if (services.GetService(typeof(IValidatorRegistry)) is not IValidatorRegistry validators)
        {
            throw new InvalidOperationException("IValidatorCollection is not registered in the service collection.");
        }

        var name = bindingContext.ModelMetadata.ParameterName;
        var untrustedInnerType = typeof(UntrustedValue<>).MakeGenericType(InnerType);
        var untrustedValue = Activator.CreateInstance(untrustedInnerType, nestedContext.Result.Model, name);

        var validator = validators.GetValidatorFor(InnerType, services);
        return [untrustedValue, validator];
    }
}