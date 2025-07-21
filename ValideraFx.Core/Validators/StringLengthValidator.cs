// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Core.Validators;

internal class StringLengthValidator(int minLength, int maxLength = int.MaxValue) : ValidatorPipeline<string>(
    new ObjectPropertyValidator<string, int>(
        x => x.Length,
        new IntegerIntervalValidator(minLength, maxLength)));
