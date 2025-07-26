// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

public class UntrustedValue<T>(T value, string? name = null)
    where T : notnull
{
    internal T Value { get; } = value;
    
    internal string? Name { get; } = name;

    public sealed override bool Equals(object? obj)
        => obj switch
        {
            T wrapped => Value.Equals(wrapped),
            UntrustedValue<T> untrusted => Value.Equals(untrusted.Value),
            _ => false
        };

    /// <remarks>
    /// The GetHashCode() method can of course be used to reverse-engineer the underlying
    /// value without validating it first, but there are easier ways than this if you really
    /// want to, f.x. reflection.
    /// </remarks>
    public sealed override int GetHashCode()
        => Value.GetHashCode();

    /// <remarks>
    /// In order to at least make it hard to bypass validation, ToString() will always throw an
    /// exception and is sealed to prevent such silliness.
    /// </remarks>>
    /// <exception cref="InvalidOperationException">Always.</exception>
    public sealed override string ToString()
        => throw new InvalidOperationException("Bypassing validation by calling ToString() is not allowed.");
}
