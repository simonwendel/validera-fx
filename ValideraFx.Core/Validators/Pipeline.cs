// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ValideraFx.Core.Validators;

/// <summary>
/// A composite validator that runs multiple <see cref="Validator{T}"/> instances in sequence.
/// </summary>
/// <typeparam name="T">The type of value to validate.</typeparam>
public class Pipeline<T> : Validator<T> where T : notnull
{
    private readonly Validator<T>[] validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pipeline{T}"/> class with the specified validators.
    /// </summary>
    /// <param name="validators">The validators to execute in sequence.</param>
    /// <exception cref="ArgumentException">Thrown if no validators are provided.</exception>
    public Pipeline(params Validator<T>[] validators)
    {
        if (validators.Length == 0)
        {
            throw new ArgumentException("A pipeline without validators is pretty useless.");
        }

        this.validators = validators;
    }
    
    /// <summary>
    /// Runs all validators in the pipeline, in order.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="name">An optional name used in validation error messages.</param>
    /// <returns><c>true</c> if all validators succeed.</returns>
    /// <remarks>
    /// The pipeline propagates <see cref="RenderValue"/> to all child validators.
    /// The first failing validator will throw a <see cref="ValidationException"/>, terminating the pipeline.
    /// </remarks>
    protected override bool Valid(T value, string? name)
    {
        foreach (var validator in validators)
        {
            validator.RenderValue = RenderValue;
        }
        
        return validators.All(validator =>
        {
            validator.Validate(new UntrustedValue<T>(value, name));
            return true;
        });
    }

    /// <summary>
    /// This method is never called. The pipeline delegates all exception messaging to its child validators.
    /// </summary>
    /// <returns>This method does not return.</returns>
    /// <remarks>
    /// The <see cref="Pipeline{T}"/> will never construct its own error messages. The first failing nested
    /// <see cref="Validator{T}"/> determines the exception and its message.
    /// </remarks>
    /// <exception cref="UnreachableException">This method should never be called.</exception>
    [ExcludeFromCodeCoverage]
    protected override string GetPartialMessage() => throw new UnreachableException();
}