// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class AsciiAlphaNumericStringValidator : Validator<string>
{
    private protected override bool Valid(string value) 
        => value.All(char.IsAscii) && value.All(char.IsLetterOrDigit);
}
