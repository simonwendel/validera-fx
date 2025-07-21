// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal interface IValidatorBase<T> where T : notnull
{
    T Validate(UntrustedValue<T> untrustedValue);
}
