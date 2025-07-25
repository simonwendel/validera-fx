// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class NonEmptyStringValidator : Validator<string>
{
    private protected override bool Valid(string value)
        => !string.IsNullOrWhiteSpace(value);
}
