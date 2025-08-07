// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core;

/// <summary>
/// A builder for registering and composing type-based validators into a single <see cref="IValidator"/> instance.
/// </summary>
public class ValidatorServiceBuilder
{
    private readonly Dictionary<Type, object> validators = new();

    /// <summary>
    /// Registers a validator type <typeparamref name="TValidator"/> for values of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to validate.</typeparam>
    /// <typeparam name="TValidator">
    /// The validator type, which must implement <see cref="IValidator{T}"/> and have a parameterless constructor.
    /// </typeparam>
    /// <returns>The same <see cref="ValidatorServiceBuilder"/> instance for chaining.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a validator for <typeparamref name="T"/> is already registered.
    /// </exception>
    public ValidatorServiceBuilder AddValidator<T, TValidator>()
        where TValidator : IValidator<T>, new() where T : notnull =>
        AddValidator(new TValidator());

    /// <summary>
    /// Registers an instance of <see cref="IValidator{T}"/> for values of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to validate.</typeparam>
    /// <param name="validator">The validator instance to register.</param>
    /// <returns>The same <see cref="ValidatorServiceBuilder"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="validator"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a validator for <typeparamref name="T"/> is already registered.
    /// </exception>
    public ValidatorServiceBuilder AddValidator<T>(IValidator<T> validator) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(validator);

        if (validators.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"A type validator for type {typeof(T)} is already registered.");
        }

        validators[typeof(T)] = validator;
        return this;
    }

    /// <summary>
    /// Builds a composed <see cref="IValidator"/> that delegates validation to the registered per-type validators.
    /// </summary>
    /// <param name="renderValues">
    /// Indicates whether error messages should include the actual value being validated.
    /// </param>
    /// <returns>A composed <see cref="IValidator"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no validators have been registered.</exception>
    public IValidator Build(bool renderValues = true)
    {
        if (validators.Count == 0)
        {
            throw new InvalidOperationException("No validators registered.");
        }

        return new ValidatorService(validators, renderValues);
    }

    private class ValidatorService(Dictionary<Type, object> validators, bool renderValues) : IValidator
    {
        public bool RenderValue { get; set; } = renderValues;

        public T Validate<T>(UntrustedValue<T> untrustedValue) where T : notnull
        {
            if (validators.TryGetValue(typeof(T), out var validator))
            {
                if (validator is not IValidator<T> typedValidator)
                {
                    throw new InvalidOperationException($"Validator for type {typeof(T)} is not of the correct type.");
                }

                typedValidator.RenderValue = RenderValue;
                return typedValidator.Validate(untrustedValue);
            }

            throw new InvalidOperationException($"No validator registered for type {typeof(T)}.");
        }
    }
}