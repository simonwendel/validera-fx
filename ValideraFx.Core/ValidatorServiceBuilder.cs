// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

namespace ValideraFx.Core;

public class ValidatorServiceBuilder
{
    private readonly Dictionary<Type, object> validators = new();

    public ValidatorServiceBuilder AddValidator<T, TValidator>()
        where TValidator : IValidator<T>, new() where T : notnull =>
        AddValidator(new TValidator());

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

    public IValidator Build()
    {
        if (validators.Count == 0)
        {
            throw new InvalidOperationException("No validators registered.");
        }

        return new ValidatorService(validators);
    }

    private class ValidatorService(Dictionary<Type, object> validators) : IValidator
    {
        public T Validate<T>(UntrustedValue<T> untrustedValue) where T : notnull
        {
            if (validators.TryGetValue(typeof(T), out var validator))
            {
                if (validator is not IValidator<T> typedValidator)
                {
                    throw new InvalidOperationException($"Validator for type {typeof(T)} is not of the correct type.");
                }

                return typedValidator.Validate(untrustedValue);
            }

            throw new InvalidOperationException($"No validator registered for type {typeof(T)}.");
        }
    }
}