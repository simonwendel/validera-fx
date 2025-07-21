// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Linq.Expressions;

namespace ValideraFx.Core.Validators;

internal class ObjectPropertyValidator<T, TProp>(Expression<Func<T, TProp>> selector, IValidatorBase<TProp> validator)
    : ValidatorBase<T>
    where T : notnull
    where TProp : notnull
{
    private protected override bool Valid(T value)
    {
        try
        {
            validator.Validate(new UntrustedValue<TProp>(selector.Compile().Invoke(value)));
            return true;
        }
        catch (ValidationException)
        {
            return false;
        }
    }
}
