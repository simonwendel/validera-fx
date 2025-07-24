// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using ValideraFx.Core.Validators;

namespace ValideraFx.Core;

public static class Limit
{
    public static IValidator<string> Length(int min, int max = int.MaxValue)
    {
        return new StringLengthValidator(min, max);
    }

    public static IValidator<int> Between(int min, int max)
    {
        return new IntegerIntervalValidator(min, max);
    }

    public static IValidator<int> AtLeast(int min)
    {
        return new IntegerIntervalValidator(min);
    }

    public static IValidator<int> AtMost(int max)
    {
        return new IntegerIntervalValidator(int.MinValue, max);
    }
    
    public static IValidator<string> ToBase64()
    {
        return new Base64AlphabetValidator();
    }
    
    public static IValidator<string> ToAlphaNumericAscii()
    {
        return new AsciiAlphaNumericStringValidator();
    }
}