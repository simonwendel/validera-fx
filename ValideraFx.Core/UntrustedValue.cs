// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

/// <summary>
/// Represents a value that must be validated before use.
///
/// <c>UntrustedValue&lt;T&gt;</c> acts as a wrapper to indicate that the contained value is not trusted. It must be
/// validated by an <see cref="IValidator"/> or <see cref="IValidator{T}"/> before it can be used safely. Direct access
/// to the underlying value is intentionally restricted until validation is performed.
/// </summary>
/// <param name="value">The value that requires validation before use.</param>
/// <param name="name">An optional name used in validation error messages.</param>
/// <typeparam name="T">The type of value to validate.</typeparam>
public sealed class UntrustedValue<T>(T value, string? name = null)
    where T : notnull
{
    /// <summary>
    /// Gets the wrapped value that requires validation before use.
    /// This property is read-only and is not externally accessible directly
    /// unless the value has been validated using an <see cref="IValidator"/>
    /// or <see cref="IValidator{T}"/>.
    /// </summary>
    internal T Value { get; } = value;

    /// <summary>
    /// Gets the name associated with this untrusted value, used in validation error messages.
    /// This property is optional and is <c>null</c> if no name was provided.
    /// </summary>
    internal string? Name { get; } = name;

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object? obj)
        => obj switch
        {
            T wrapped => Value.Equals(wrapped),
            UntrustedValue<T> untrusted => Value.Equals(untrusted.Value),
            _ => false
        };

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <remarks>
    /// The GetHashCode() method can of course be used to reverse-engineer the underlying value without validating it
    /// first, but there are easier ways than this if you really want to.
    /// </remarks>
    /// <returns>
    /// A hash code for the current object.
    /// </returns>
    public override int GetHashCode()
        => Value.GetHashCode();

    /// <summary>
    /// Returns a string that represents this wrapper type.
    /// </summary>
    /// <returns>
    /// The name of the wrapper's type.
    /// </returns>
    public override string ToString() => FormatType(GetType());

    private static string FormatType(Type type)
    {
        if (!type.IsGenericType)
        {
            return type.FullName ?? type.Name;
        }

        var arguments = type.GetGenericArguments().Select(argument => $"{FormatType(argument)}");
        return $"{type.Name}[{string.Join(",", arguments)}]";
    }
}