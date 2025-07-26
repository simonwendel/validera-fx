// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ValideraFx.Web;

internal class UntrustedValueValidator : IModelValidator
{
    public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context) => [];
}