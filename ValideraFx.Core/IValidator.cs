// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

public interface IValidator<T> where T : notnull 
{
    bool RenderValue { get; set; }
    T Validate(UntrustedValue<T> untrustedValue);
}

public interface IValidator
{
    bool RenderValue { get; set; }
    T Validate<T>(UntrustedValue<T> untrustedValue) where T : notnull;
}