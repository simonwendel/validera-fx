﻿// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal abstract class ValidatorBase<T> : IValidatorBase<T> where T : notnull
{
    public T Validate(UntrustedValue<T> untrustedValue)
        => Valid(untrustedValue.Value)
            ? untrustedValue.Value
            : throw new ValidationException();

    private protected abstract bool Valid(T value);
}
