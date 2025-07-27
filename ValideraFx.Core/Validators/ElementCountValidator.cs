// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class ElementCountValidator<T> : Validator<IEnumerable<T>>
    where T : notnull
{
    private readonly int minCount;
    private readonly int maxCount;
    private readonly ObjectValidator<IList<T>, int> elementCountValidator;

    public ElementCountValidator(int minCount, int maxCount = int.MaxValue)
    {
        if (maxCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxCount), "Maximum count cannot be negative.");
        }

        if (minCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minCount), "Minimum count cannot be negative.");
        }

        if (maxCount < minCount)
        {
            throw new ArgumentOutOfRangeException(nameof(minCount), "Maximum count cannot be less than minimum count.");
        }

        this.minCount = minCount;
        this.maxCount = maxCount;

        elementCountValidator = new ObjectValidator<IList<T>, int>(
            x => x.Count,
            new IntegerIntervalValidator(minCount, maxCount));
    }

    protected override bool Valid(IEnumerable<T> value, string? name)
    {
        try
        {
            elementCountValidator.Validate(new UntrustedValue<IList<T>>(value.ToList(), name));
            return true;
        }
        catch (ValidationException)
        {
            return false;
        }
    }

    protected override string GetValueMessage(UntrustedValue<IEnumerable<T>> untrustedValue)
    {
        var elementWord = untrustedValue.Value.Count() == 1 ? "element" : "elements";
        return $"The list with {untrustedValue.Value.Count()} {elementWord}";
    }

    protected override string GetPartialMessage()
    {
        var maxLengthLabel = maxCount == int.MaxValue ? "int.MaxValue" : $"{maxCount}";
        return $"does not have a valid length (must be between {minCount} and {maxLengthLabel})";
    }
}