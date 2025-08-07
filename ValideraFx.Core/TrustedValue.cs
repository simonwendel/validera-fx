// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

public sealed class TrustedValue<T> where T : notnull
{
    public TrustedValue(UntrustedValue<T> value, IValidator<T> validator)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(validator);
        Value = validator.Validate(value);
        Name = value.Name;
    }

    public T Value { get; }

    public string? Name { get; }

    public override bool Equals(object? obj) => Value.Equals(obj);

    public override int GetHashCode() => Value.GetHashCode();

    public override string? ToString() => Value.ToString();
}