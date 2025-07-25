// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class StringLengthValidator : Pipeline<string>
{
    public StringLengthValidator(int minLength, int maxLength = int.MaxValue) : base(new ObjectPropertyValidator<string, int>(
        x => x.Length,
        new IntegerIntervalValidator(minLength, maxLength)))
    {
        if (maxLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative.");
        }
        
        if (minLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length cannot be negative.");
        }

        if (maxLength < minLength)
        {
            throw new ArgumentException("Maximum length cannot be less than minimum length.", nameof(maxLength));
        }
    }
}
