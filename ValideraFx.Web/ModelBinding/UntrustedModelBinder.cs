// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal class UntrustedModelBinder(
    Type innerType,
    IModelMetadataProvider metadataProvider,
    IModelBinderFactory binderFactory)
    : ModelBinderBase(innerType, metadataProvider, binderFactory)
{
    protected override Type GetTypeToBind() => typeof(UntrustedValue<>).MakeGenericType(InnerType);

    protected override object?[] GetConstructorArguments(
        ModelBindingContext bindingContext,
        ModelBindingContext nestedContext) =>
        [nestedContext.Result.Model, bindingContext.ModelMetadata.ParameterName];
}