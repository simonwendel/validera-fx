// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

/// <summary>
/// Base class for implementing typed validators using a simple boolean condition and validation message.
/// </summary>
/// <typeparam name="T">The type of value to validate.</typeparam>
public abstract class Validator<T> : IValidator<T> where T : notnull
{
    /// <summary>
    /// Validates the given untrusted value.
    /// </summary>
    /// <param name="untrustedValue">The untrusted value to validate.</param>
    /// <returns>The validated value if it passes the validation rule.</returns>
    /// <exception cref="ValidationException">Thrown if the value fails validation.</exception>
    public T Validate(UntrustedValue<T> untrustedValue)
        => Valid(untrustedValue.Value, untrustedValue.Name)
            ? untrustedValue.Value
            : throw ValidationFailed(untrustedValue);

    /// <summary>
    /// When implemented in a derived class, determines whether the given value passes validation.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">An optional name used in validation error messages.</param>
    /// <returns><c>true</c> if the value is valid; otherwise, <c>false</c>.</returns>
    protected abstract bool Valid(T value, string? name);
    
    /// <summary>
    /// Gets or sets a value indicating whether the validated value should be included in validation messages.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, error messages will include the actual value (e.g., "The value 'abc' must be...").
    /// When <c>false</c>, messages will refer to "The value" without showing the contents.
    /// </remarks>
    public bool RenderValue { get; set; } = true;

    protected virtual string GetValidationMessage(UntrustedValue<T> untrustedValue)
    {
        var valueMessage = "The value";
        if (RenderValue)
        {
            valueMessage = GetValueMessage(untrustedValue);
        }
        
        return $"{valueMessage} {GetPartialMessage()}.";
    }

    protected virtual string GetValueMessage(UntrustedValue<T> untrustedValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return untrustedValue.Value is null ? "The null value" : $"The value '{untrustedValue.Value}'";
    }

    protected abstract string GetPartialMessage();

    private ValidationException ValidationFailed(UntrustedValue<T> untrustedValue) =>
        new(untrustedValue.Name, GetValidationMessage(untrustedValue));
}