// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal abstract class Validator<T> : IValidator<T> where T : notnull
{
    public T Validate(UntrustedValue<T> untrustedValue)
        => Valid(untrustedValue.Value)
            ? untrustedValue.Value
            : throw new ValidationException();

    private protected abstract bool Valid(T value);
}
