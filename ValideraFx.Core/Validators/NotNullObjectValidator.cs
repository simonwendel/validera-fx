// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics;

namespace ValideraFx.Core.Validators;

public class NotNullObjectValidator<T> : Validator<T> where T : notnull
{
    protected override bool Valid(T? value, string? name) => value is not null;

    protected override string GetValidationMessage(UntrustedValue<T> untrustedValue) => "The value must not be null.";

    protected override string GetPartialMessage() => throw new UnreachableException();
}