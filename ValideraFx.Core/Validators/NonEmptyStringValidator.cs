// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class NonEmptyStringValidator : Validator<string>
{
    protected override bool Valid(string value, string? name)
        => !string.IsNullOrWhiteSpace(value);

    protected override string GetPartialMessage() => "is null or empty";
}