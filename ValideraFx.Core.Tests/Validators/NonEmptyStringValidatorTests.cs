// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class NonEmptyStringValidatorTests
{
    [Theory]
    [InlineAutoData(null)]
    internal void Validate_GivenNullString_ThrowsException(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The null value is null or empty.");
    }
    
    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    internal void Validate_GivenEmptyString_ThrowsException(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value '{value}' is null or empty.");
    }

    [Theory]
    [InlineAutoData(null)]
    internal void Validate_GivenNullStringAndName_ThrowsException(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myString'. The null value is null or empty.");
    }

    [Theory]
    [InlineAutoData("")]
    [InlineAutoData(" ")]
    internal void Validate_GivenEmptyStringAndName_ThrowsException(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myString'. The value '{value}' is null or empty.");
    }

    [Theory, AutoData]
    internal void Validate_GivenStringWithContents_ReturnsValue(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        sut.Validate(untrusted).Should().Be(value);
    }
}