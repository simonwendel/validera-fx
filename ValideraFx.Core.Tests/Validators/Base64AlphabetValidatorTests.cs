// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class Base64AlphabetValidatorTests
{
    [Theory]
    [InlineAutoData("-")]
    [InlineAutoData("\\")]
    [InlineAutoData("å")]
    [InlineAutoData(";")]
    internal void Validate_GivenIllegalCharacter_ThrowsException(string value, Base64AlphabetValidator sut)
    {
        Action validating = () => sut.Validate(new UntrustedValue<string>(value));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed, the value '{value}' is not a valid base64 alphabet string.");
    }

    [Theory]
    [InlineAutoData("-")]
    [InlineAutoData("\\")]
    [InlineAutoData("å")]
    [InlineAutoData(";")]
    internal void Validate_GivenIllegalCharacterAndName_ThrowsException(string value, Base64AlphabetValidator sut)
    {
        Action validating = () => sut.Validate(new UntrustedValue<string>(value, "myString"));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myString', the value '{value}' is not a valid base64 alphabet string.");
    }

    [Theory]
    [InlineAutoData("+/")]
    [InlineAutoData("+/=")]
    [InlineAutoData("+/===")]
    [InlineAutoData("+/====")]
    internal void Validate_GivenInvalidPadding_ThrowsException(string value, Base64AlphabetValidator sut)
    {
        Action validating = () => sut.Validate(new UntrustedValue<string>(value));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed, the value '{value}' is not a valid base64 alphabet string.");
    }

    [Theory]
    [InlineAutoData("+/")]
    [InlineAutoData("+/=")]
    [InlineAutoData("+/===")]
    [InlineAutoData("+/====")]
    internal void Validate_GivenInvalidPaddingAndName_ThrowsException(string value, Base64AlphabetValidator sut)
    {
        Action validating = () => sut.Validate(new UntrustedValue<string>(value, "myString"));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myString', the value '{value}' is not a valid base64 alphabet string.");
    }

    [Theory]
    [InlineAutoData("ABCDEFGHIJKLMNOPQRSTUVWXYZ==")]
    [InlineAutoData("abcdefghijklmnopqrstuvwxyz==")]
    [InlineAutoData("0123456789==")]
    [InlineAutoData("+/==")]
    [InlineAutoData(" ")]
    [InlineAutoData("")]
    internal void Validate_GivenValidString_ReturnsValue(string value, Base64AlphabetValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        sut.Validate(untrusted).Should().Be(value);
    }
}