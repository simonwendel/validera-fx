// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

public class IntegerIntervalValidator : Validator<int>
{
    private readonly int lowerBounds;
    private readonly int upperBounds;

    public IntegerIntervalValidator(int lowerBounds, int upperBounds = int.MaxValue)
    {
        if (upperBounds < lowerBounds)
        {
            throw new ArgumentException();
        }

        this.lowerBounds = lowerBounds;
        this.upperBounds = upperBounds;
    }

    private protected override bool Valid(int value)
    {
        return value >= lowerBounds && value <= upperBounds;
    }
}
