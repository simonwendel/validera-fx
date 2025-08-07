// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

/// <summary>
/// Represents a value that has been successfully validated using an <see cref="IValidator{T}"/>.
/// 
/// <c>TrustedValue&lt;T&gt;</c> represents a value that has been validated and is safe to use. It is produced by
/// validating an <see cref="UntrustedValue{T}"/> using an <see cref="IValidator"/> or <see cref="IValidator{T}"/>.
/// Access to the validated value is safe and unrestricted, as validation has already been enforced.
/// </summary>
/// <typeparam name="T">The type of the validated value.</typeparam>
public sealed class TrustedValue<T> where T : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrustedValue{T}"/> class by validating the specified untrusted
    /// value.
    /// </summary>
    /// <param name="value">The untrusted value to validate.</param>
    /// <param name="validator">The validator used to validate the untrusted value.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if either <paramref name="value"/> or <paramref name="validator"/> is <c>null</c>.
    /// </exception>
    public TrustedValue(UntrustedValue<T> value, IValidator<T> validator)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(validator);
        Value = validator.Validate(value);
        Name = value.Name;
    }

    /// <summary>
    /// Gets the validated value.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets the name associated with this trusted value. This property is optional and is <c>null</c> if no name was
    /// provided.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Determines whether the specified object is equal to the current validated value.
    /// </summary>
    /// <param name="obj">The object to compare with the current validated value.</param>
    /// <returns><c>true</c> if the specified object is equal to the validated value; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => Value.Equals(obj);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the validated value.</returns>

    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Returns a string that represents the validated value.
    /// </summary>
    /// <returns>A string representation of the validated value.</returns>
    public override string? ToString() => Value.ToString();
}