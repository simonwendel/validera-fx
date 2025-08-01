// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ValideraFx.Core.Validators;

internal class NonEmptyStringValidator : Validator<string>
{
    protected override bool Valid(string value, string? name)
        => !string.IsNullOrWhiteSpace(value);

    protected override string GetValidationMessage(UntrustedValue<string> untrustedValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return untrustedValue.Value is null
            ? "The value is null."
            : "The value is empty.";
    }
    
    [ExcludeFromCodeCoverage]
    protected override string GetPartialMessage() => throw new UnreachableException();
}