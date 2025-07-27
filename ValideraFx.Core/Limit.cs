// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using ValideraFx.Core.Validators;

namespace ValideraFx.Core;

public static class Limit
{
    public static IValidator<string> Length(int min, int max = int.MaxValue) => new StringLengthValidator(min, max);

    public static IValidator<int> AtLeast(int min) => new IntegerIntervalValidator(min);

    public static IValidator<int> AtMost(int max) => new IntegerIntervalValidator(int.MinValue, max);

    public static IValidator<int> Within(int min, int max) => new IntegerIntervalValidator(min, max);

    public static IValidator<string> ToBase64() => new Base64AlphabetValidator();

    public static IValidator<string> ToAlphaNumericAscii() => new AsciiAlphaNumericStringValidator();

    public static IValidator<string> ToNonEmptyString() => new NonEmptyStringValidator();

    public static IValidator<IEnumerable<T>> CountAtLeast<T>(int min) where T : notnull =>
        new ElementCountValidator<T>(min);

    public static IValidator<IEnumerable<T>> CountAtMost<T>(int max) where T : notnull =>
        new ElementCountValidator<T>(0, max);

    public static IValidator<IEnumerable<T>> CountWithin<T>(int min, int max) where T : notnull =>
        new ElementCountValidator<T>(min, max);
}