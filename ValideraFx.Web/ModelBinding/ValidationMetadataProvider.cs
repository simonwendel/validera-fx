// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal class ValidationMetadataProvider : IValidationMetadataProvider
{
    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        var type = context.Key.ModelType;
        if (type.IsGenericType
            && (type.GetGenericTypeDefinition() == typeof(UntrustedValue<>)
                || type.GetGenericTypeDefinition() == typeof(TrustedValue<>)))
        {
            context.ValidationMetadata.ValidateChildren = false;
        }
    }
}