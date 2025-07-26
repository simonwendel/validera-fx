// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class AsciiAlphaNumericStringValidatorTests
{
    [Theory]
    [InlineAutoData(" ")]
    [InlineAutoData("å")]
    [InlineAutoData(";")]
    [InlineAutoData("123_")]
    [InlineAutoData("😊")]
    internal void Validate_GivenStringWithNonAlphanumericCharacters_ThrowsException(string value,
        AsciiAlphaNumericStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value '{value}' is not a valid ASCII alpha-numeric string.");
    }

    [Theory]
    [InlineAutoData(" ")]
    [InlineAutoData("å")]
    [InlineAutoData(";")]
    [InlineAutoData("123_")]
    [InlineAutoData("😊")]
    internal void Validate_GivenStringWithNonAlphanumericCharactersAndValueName_ThrowsException(string value,
        AsciiAlphaNumericStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myString'. The value '{value}' is not a valid ASCII alpha-numeric string.");
    }

    [Theory]
    [InlineAutoData("1")]
    [InlineAutoData("a")]
    [InlineAutoData("0123456789abcdefghijklmnopqrstuvxyz")]
    internal void Validate_GivenStringWithContents_ReturnsValue(string value, AsciiAlphaNumericStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        sut.Validate(untrusted).Should().Be(value);
    }
}