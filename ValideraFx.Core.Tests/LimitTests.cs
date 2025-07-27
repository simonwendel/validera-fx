// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests;

public class LimitTests
{
    [Fact]
    public void Length_GivenBounds_ReturnsValidator() =>
        Limit.Length(1, 10).Should().BeOfType<StringLengthValidator>();

    [Fact]
    public void AtLeast_GivenBounds_ReturnsValidator() =>
        Limit.AtLeast(1).Should().BeOfType<IntegerIntervalValidator>();

    [Fact]
    public void AtMost_GivenBounds_ReturnsValidator() =>
        Limit.AtMost(10).Should().BeOfType<IntegerIntervalValidator>();

    [Fact]
    public void Within_GivenBounds_ReturnsValidator() =>
        Limit.Within(1, 10).Should().BeOfType<IntegerIntervalValidator>();

    [Fact]
    public void AsBase64_Always_ReturnsValidator() =>
        Limit.ToBase64().Should().BeOfType<Base64AlphabetValidator>();

    [Fact]
    public void AsAlphaNumericAscii_Always_ReturnsValidator() =>
        Limit.ToAlphaNumericAscii().Should().BeOfType<AsciiAlphaNumericStringValidator>();

    [Fact]
    public void ToNonEmptyString_Always_ReturnsValidator() =>
        Limit.ToNonEmptyString().Should().BeOfType<NonEmptyStringValidator>();
    
    [Fact]
    public void CountAtLeast_GivenBounds_ReturnsValidator() =>
        Limit.CountAtLeast<string>(1).Should().BeOfType<ElementCountValidator<string>>();

    [Fact]
    public void CountAtMost_GivenBounds_ReturnsValidator() =>
        Limit.CountAtMost<string>(10).Should().BeOfType<ElementCountValidator<string>>();
    
    [Fact]
    public void CountWithin_GivenBounds_ReturnsValidator() =>
        Limit.CountWithin<string>(1, 10).Should().BeOfType<ElementCountValidator<string>>();
}