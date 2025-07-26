// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

public class ValidationException(string? validatedName, string validationMessage) : Exception
{
    public string? ValidatedName { get; } = validatedName;
    
    public string ValidationMessage { get; } = validationMessage;

    public override string Message => ValidatedName is null
        ? $"Validation failed. {ValidationMessage}"
        : $"Validation failed for '{ValidatedName}'. {ValidationMessage}";
}