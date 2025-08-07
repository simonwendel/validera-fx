// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

/// <summary>
/// Defines a validator for values of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of value to validate.</typeparam>
/// <remarks>
/// Implementations must enforce validation logic and may include the value itself in error messages, depending on the
/// value of <see cref="RenderValue"/>.
/// </remarks>
public interface IValidator<T> where T : notnull 
{
    /// <summary>
    /// Gets or sets a value indicating whether the validated value should be included in validation messages.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, error messages will include the actual value (e.g., "The value 'abc' must be...").
    /// When <c>false</c>, messages will refer to "The value" without showing the contents.
    /// </remarks>
    bool RenderValue { get; set; }
    
    /// <summary>
    /// Validates the specified untrusted value.
    /// </summary>
    /// <param name="untrustedValue">The untrusted value to validate.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ValidationException">Thrown if validation fails.</exception>
    T Validate(UntrustedValue<T> untrustedValue);
}

/// <summary>
/// Defines a non-generic validator capable of validating values of arbitrary types at runtime.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// Gets or sets a value indicating whether the validated value should be included in validation messages.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, error messages will include the actual value (e.g., "The value 'abc' must be...").
    /// When <c>false</c>, messages will refer to "The value" without showing the contents.
    /// </remarks>
    bool RenderValue { get; set; }
    
    /// <summary>
    /// Validates the specified untrusted value using the appropriate validator for its type.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    /// <param name="untrustedValue">The untrusted value to validate.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ValidationException">Thrown if validation fails.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no validator is registered for the specified type.
    /// </exception>
    T Validate<T>(UntrustedValue<T> untrustedValue) where T : notnull;
}