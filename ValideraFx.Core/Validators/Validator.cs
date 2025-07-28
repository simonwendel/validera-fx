// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

public abstract class Validator<T> : IValidator<T> where T : notnull
{
    public T Validate(UntrustedValue<T> untrustedValue)
        => Valid(untrustedValue.Value, untrustedValue.Name)
            ? untrustedValue.Value
            : throw ValidationFailed(untrustedValue);

    protected abstract bool Valid(T value, string? name);

    protected virtual string GetValidationMessage(UntrustedValue<T> untrustedValue) =>
        $"{GetValueMessage(untrustedValue)} {GetPartialMessage()}.";

    protected virtual string GetValueMessage(UntrustedValue<T> untrustedValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return untrustedValue.Value is null ? "The null value" : $"The value '{untrustedValue.Value}'";
    }

    protected abstract string GetPartialMessage();

    private ValidationException ValidationFailed(UntrustedValue<T> untrustedValue) =>
        new(untrustedValue.Name, GetValidationMessage(untrustedValue));
}