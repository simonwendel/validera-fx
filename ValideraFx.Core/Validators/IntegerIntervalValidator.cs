// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class IntegerIntervalValidator : Validator<int>
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

    private protected override bool Valid(int value, string? name)
    {
        return value >= lowerBounds && value <= upperBounds;
    }

    private protected override string GetPartialMessage()
    {
        var lowerBoundsLabel = lowerBounds == int.MinValue ? "int.MinValue" : $"{lowerBounds}";
        var upperBoundsLabel = upperBounds == int.MaxValue ? "int.MaxValue" : $"{upperBounds}";
        return $"is not within the interval [{lowerBoundsLabel}, {upperBoundsLabel}]";
    }
}