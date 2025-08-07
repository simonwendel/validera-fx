// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

/// <summary>
/// The exception that is thrown when validation fails for a value.
/// </summary>
/// <param name="validatedName">
/// The name of the value that failed validation, used to clarify the context of the error.
/// </param>
/// <param name="validationMessage">
/// A message that describes the reason for the validation failure.
/// </param>
public class ValidationException(string? validatedName, string validationMessage) : Exception
{
    /// <summary>
    /// Gets the name of the value that failed validation.
    /// </summary>
    public string? ValidatedName { get; } = validatedName;

    /// <summary>
    /// Gets the message that describes the specific validation failure.
    /// </summary>
    public string ValidationMessage { get; } = validationMessage;

    /// <summary>
    /// Gets a message that describes the current exception.
    /// </summary>
    public override string Message => ValidatedName is null
        ? $"Validation failed. {ValidationMessage}"
        : $"Validation failed for '{ValidatedName}'. {ValidationMessage}";
}