// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal abstract class ValidatorPipeline<T> : Validator<T> where T : notnull
{
    private readonly Validator<T>[] validators;

    private protected ValidatorPipeline(params Validator<T>[] validators)
    {
        if (validators.Length == 0)
        {
            throw new ArgumentException("A pipeline without validators is pretty useless.");
        }
        
        this.validators = validators;
    }

    private protected override bool Valid(T value)
        => validators.All(validator =>
        {
            validator.Validate(new UntrustedValue<T>(value));
            return true;
        });
}
