// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using ValideraFx.Core.Validators;

namespace ValideraFx.Core;

/// <summary>
/// Provides a set of factory methods for common validation rules and constraints.
/// </summary>
public static class Limit
{
    /// <summary>
    /// Creates a validator that checks whether a string's length is within the specified range.
    /// </summary>
    /// <param name="min">The minimum allowed length (inclusive).</param>
    /// <param name="max">The maximum allowed length (inclusive). Defaults to <see cref="int.MaxValue"/>.</param>
    /// <returns>An <see cref="IValidator{String}"/> that validates string length.</returns>
    public static IValidator<string> Length(int min, int max = int.MaxValue) => new StringLengthValidator(min, max);

    /// <summary>
    /// Creates a validator that checks whether an integer is greater than or equal to the specified minimum.
    /// </summary>
    /// <param name="min">The minimum allowed value (inclusive).</param>
    /// <returns>An <see cref="IValidator{Int32}"/> that validates lower bound.</returns>
    public static IValidator<int> AtLeast(int min) => new IntegerIntervalValidator(min);
    
    /// <summary>
    /// Creates a validator that checks whether an integer is less than or equal to the specified maximum.
    /// </summary>
    /// <param name="max">The maximum allowed value (inclusive).</param>
    /// <returns>An <see cref="IValidator{Int32}"/> that validates upper bound.</returns>
    public static IValidator<int> AtMost(int max) => new IntegerIntervalValidator(int.MinValue, max);

    /// <summary>
    /// Creates a validator that checks whether an integer is within the specified range.
    /// </summary>
    /// <param name="min">The minimum allowed value (inclusive).</param>
    /// <param name="max">The maximum allowed value (inclusive).</param>
    /// <returns>An <see cref="IValidator{Int32}"/> that validates the value range.</returns>
    public static IValidator<int> Within(int min, int max) => new IntegerIntervalValidator(min, max);
    
    /// <summary>
    /// Creates a validator that checks whether a string contains only valid Base64 characters.
    /// </summary>
    /// <returns>An <see cref="IValidator{String}"/> for Base64 input.</returns>
    public static IValidator<string> ToBase64() => new Base64AlphabetValidator();

    /// <summary>
    /// Creates a validator that checks whether a string contains only ASCII alphanumeric characters.
    /// </summary>
    /// <returns>An <see cref="IValidator{String}"/> for ASCII alphanumeric input.</returns>
    public static IValidator<string> ToAlphaNumericAscii() => new AsciiAlphaNumericStringValidator();
    
    /// <summary>
    /// Creates a validator that checks whether a string is non-empty.
    /// </summary>
    /// <returns>An <see cref="IValidator{String}"/> that enforces non-empty input.</returns>
    public static IValidator<string> ToNonEmptyString() => new NonEmptyStringValidator();

    /// <summary>
    /// Creates a validator that checks whether a sequence contains at least the specified number of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="min">The minimum number of elements required.</param>
    /// <returns>An <see cref="IValidator{IEnumerable{T}}"/> that validates minimum count.</returns>
    public static IValidator<IEnumerable<T>> CountAtLeast<T>(int min) where T : notnull =>
        new ElementCountValidator<T>(min);

    /// <summary>
    /// Creates a validator that checks whether a sequence contains at most the specified number of elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="max">The maximum number of elements allowed.</param>
    /// <returns>An <see cref="IValidator{IEnumerable{T}}"/> that validates maximum count.</returns>
    public static IValidator<IEnumerable<T>> CountAtMost<T>(int max) where T : notnull =>
        new ElementCountValidator<T>(0, max);

    /// <summary>
    /// Creates a validator that checks whether a sequence contains a number of elements within the specified range.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="min">The minimum number of elements required.</param>
    /// <param name="max">The maximum number of elements allowed.</param>
    /// <returns>An <see cref="IValidator{IEnumerable{T}}"/> that validates element count range.</returns>
    public static IValidator<IEnumerable<T>> CountWithin<T>(int min, int max) where T : notnull =>
        new ElementCountValidator<T>(min, max);
}