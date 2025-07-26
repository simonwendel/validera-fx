// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

public abstract class Validator<T> : IValidator<T> where T : notnull
{
    public T Validate(UntrustedValue<T> untrustedValue)
        => Valid(untrustedValue.Value, untrustedValue.Name)
            ? untrustedValue.Value
            : throw ValidationFailed(untrustedValue);

    private protected abstract bool Valid(T value, string? name);
    private protected abstract string GetPartialMessage();

    private ValidationException ValidationFailed(UntrustedValue<T> untrustedValue)
    {
        var message = $"The value '{untrustedValue.Value}' {GetPartialMessage()}.";
        return new ValidationException(untrustedValue.Name, message);
    }
}