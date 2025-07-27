// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ValideraFx.Core;

namespace ValideraFx.Web.ModelBinding;

internal class ModelValidatorProvider : IModelValidatorProvider
{
    public void CreateValidators(ModelValidatorProviderContext context)
    {
        var type = context.ModelMetadata.ModelType;
        if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(UntrustedValue<>))
        {
            return;
        }

        context.Results.Clear();
        context.Results.Add(new ValidatorItem
        {
            Validator = new NoOpModelValidator(),
            IsReusable = true
        });
    }
}