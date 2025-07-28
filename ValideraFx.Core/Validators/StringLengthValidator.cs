// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class StringLengthValidator : Validator<string>
{
    private readonly int minLength;
    private readonly int maxLength;
    private readonly Validator<string> stringValidator;

    public StringLengthValidator(int minLength, int maxLength = int.MaxValue)
    {
        if (maxLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative.");
        }

        if (minLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length cannot be negative.");
        }

        this.minLength = minLength;
        this.maxLength = maxLength;

        stringValidator = new Pipeline<string>(
            new NotNullObjectValidator<string>(),
            new ObjectValidator<string, int>(x => x.Length, new IntegerIntervalValidator(minLength, maxLength)));
    }

    protected override bool Valid(string value, string? name)
    {
        try
        {
            stringValidator.Validate(new UntrustedValue<string>(value, name));
            return true;
        }
        catch (ValidationException)
        {
            return false;
        }
    }

    protected override string GetPartialMessage()
    {
        var maxLengthLabel = maxLength == int.MaxValue ? "int.MaxValue" : $"{maxLength}";
        return $"does not have a valid length (must be between {minLength} and {maxLengthLabel})";
    }
}