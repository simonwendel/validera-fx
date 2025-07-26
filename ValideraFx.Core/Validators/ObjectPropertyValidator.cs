// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics;
using System.Linq.Expressions;

namespace ValideraFx.Core.Validators;

internal class ObjectPropertyValidator<T, TProp>(Expression<Func<T, TProp>> selector, IValidator<TProp> validator)
    : Validator<T>
    where T : notnull
    where TProp : notnull
{
    private protected override bool Valid(T value, string? name)
    {
        validator.Validate(new UntrustedValue<TProp>(
            selector.Compile().Invoke(value),
            $"{GetMemberName(selector, name)}"));
        return true;
    }

    private static string GetMemberName(Expression<Func<T, TProp>> selector, string? name)
    {
        var body = selector.Body is UnaryExpression u ? u.Operand : selector.Body;
        switch (body)
        {
            case MemberExpression member:
                var prefix = name != null ? $"{name}." : string.Empty;
                return $"{prefix}{member.Member.Name}";
            case ParameterExpression:
                return typeof(TProp).IsPrimitive
                    ? (name != null ? $"{name}" : string.Empty)
                    : throw new ArgumentException("Bare parameter allowed only for primitive types");
            default:
                throw new ArgumentException("Selector must be a member expression or a bare parameter");
        }
    }

    /// <remarks>
    /// The <see cref="ObjectPropertyValidator{T,TProp}" /> will never throw an exception itself, but instead
    /// the nested <see cref="IValidator{TProp}" /> will throw an exception if the validation fails. The nested
    /// validator is thus responsible for providing the error message.
    /// </remarks>
    /// <throws cref="UnreachableException">This method should never be called.</throws>
    private protected override string GetPartialMessage() => throw new UnreachableException();
}