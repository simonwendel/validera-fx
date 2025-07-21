// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class StringLengthValidator : ValidatorPipelineBase<string>
{
    public StringLengthValidator(int minLength, int maxLength = int.MaxValue)
        : base(
            new ObjectPropertyValidator<string, int>(
                x => x.Length,
                new IntegerIntervalValidator(minLength, maxLength)))
    {
    }
}
