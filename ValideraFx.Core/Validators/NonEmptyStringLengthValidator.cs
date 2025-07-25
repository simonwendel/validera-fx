// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class NonEmptyStringLengthValidator : Pipeline<string>
{
    public NonEmptyStringLengthValidator(int minLength, int maxLength = int.MaxValue)
        : base(new NonEmptyStringValidator(), new StringLengthValidator(minLength, maxLength))
    {
    }
}
