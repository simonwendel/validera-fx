// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class Base64AlphabetValidator : Validator<string>
{
    private protected override bool Valid(string value)
        => Convert.TryFromBase64String(value, new byte[value.Length * 2], out _); // BMP is max 2 bytes per character
}
