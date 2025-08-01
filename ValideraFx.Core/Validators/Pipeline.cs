// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ValideraFx.Core.Validators;

public class Pipeline<T> : Validator<T> where T : notnull
{
    private readonly Validator<T>[] validators;

    public Pipeline(params Validator<T>[] validators)
    {
        if (validators.Length == 0)
        {
            throw new ArgumentException("A pipeline without validators is pretty useless.");
        }

        this.validators = validators;
    }

    protected override bool Valid(T value, string? name)
    {
        foreach (var validator in validators)
        {
            validator.RenderValue = RenderValue;
        }
        
        return validators.All(validator =>
        {
            validator.Validate(new UntrustedValue<T>(value, name));
            return true;
        });
    }

    /// <remarks>
    /// The <see cref="Pipeline{T}" /> will never throw an exception itself, but instead
    /// the first nested <see cref="IValidator{TProp}" /> that throws will control the exception. That nested
    /// validator is thus responsible for providing the error message.
    /// </remarks>
    /// <exception cref="UnreachableException">This method should never be called.</exception>
    [ExcludeFromCodeCoverage]
    protected override string GetPartialMessage() => throw new UnreachableException();
}