// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class AsciiAlphaNumericStringValidator : Validator<string>
{
    protected override bool Valid(string value, string? name)
        => value.All(char.IsAscii) && value.All(char.IsLetterOrDigit);

    protected override string GetPartialMessage() => "is not a valid ASCII alpha-numeric string";
}