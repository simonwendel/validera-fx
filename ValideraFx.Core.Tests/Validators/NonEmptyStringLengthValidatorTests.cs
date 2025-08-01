// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class NonEmptyStringLengthValidatorTests
{
    private readonly NonEmptyStringLengthValidator sut = new(3, 5);

    [Theory]
    [InlineData("    ")]
    [InlineData("\t\t\t\t")]
    internal void Validate_GivenEmptyOrWhitespaceString_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value is empty.");
    }

    [Theory]
    [InlineAutoData(null)]
    internal void Validate_GivenNullString_ThrowsException(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value is null.");
    }

    [Theory]
    [InlineData("    ")]
    [InlineData("\t\t\t\t")]
    internal void Validate_GivenEmptyOrWhitespaceStringAndName_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myString'. The value is empty.");
    }

    [Theory]
    [InlineAutoData(null)]
    internal void Validate_GivenNullStringAndName_ThrowsException(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myString'. The value is null.");
    }

    [Theory]
    [InlineData("12")]
    [InlineData("123456")]
    internal void Validate_GivenNonValidLengthString_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value '{value}' does not have a valid length (must be between 3 and 5).");
    }

    [Fact]
    internal void Validate_GivenTooWayLongString_ThrowsExceptionWithTruncatedValue()
    {
        const string value = "abcdefghijklmnopqrstuvxyz1234567890abcdefghijklmnopqrstuvxyz1234567890abcdefghijklmno";
        const string renderedValue = "abcdefghijklmnopqrstuvxyz1234567890abcdefghijklmnopqrstuvxyz...";
        var untrusted = new UntrustedValue<string>(value);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value '{renderedValue}' does not have a valid length (must be between 3 and 5).");
    }

    [Theory]
    [InlineData("12")]
    [InlineData("123456")]
    internal void Validate_GivenNonValidLengthStringAndName_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myString'. The value '{value}' does not have a valid length (must be between 3 and 5).");
    }

    [Fact]
    internal void Validate_GivenTooWayLongStringAndName_ThrowsExceptionWithTruncatedValue()
    {
        const string value = "abcdefghijklmnopqrstuvxyz1234567890abcdefghijklmnopqrstuvxyz1234567890abcdefghijklmno";
        const string renderedValue = "abcdefghijklmnopqrstuvxyz1234567890abcdefghijklmnopqrstuvxyz...";
        var untrusted = new UntrustedValue<string>(value, "myString");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myString'. The value '{renderedValue}' does not have a valid length (must be between 3 and 5).");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345")]
    [InlineData("   1")]
    [InlineData("\t\ta\t")]
    internal void Validate_GivenValidString_ReturnsValue(string value)
    {
        var untrusted = new UntrustedValue<string>(value);
        sut.Validate(untrusted).Should().Be(value);
    }
}