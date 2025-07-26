// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class NonEmptyStringLengthValidatorTests
{
    [Theory]
    [InlineData("    ")]
    [InlineData("\t\t\t\t")]
    internal void Validate_GivenEmptyOrWhitespaceString_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value);
        var sut = new NonEmptyStringLengthValidator(3, 5);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed, the value '{value}' is null or empty.");
    }

    [Theory]
    [InlineData("    ")]
    [InlineData("\t\t\t\t")]
    internal void Validate_GivenEmptyOrWhitespaceStringAndName_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        var sut = new NonEmptyStringLengthValidator(3, 5);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myString', the value '{value}' is null or empty.");
    }

    [Theory]
    [InlineData("12")]
    [InlineData("123456")]
    internal void Validate_GivenNonValidLengthString_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value);
        var sut = new NonEmptyStringLengthValidator(3, 5);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed, the value '{value}' does not have a valid length (must be between 3 and 5).");
    }

    [Theory]
    [InlineData("12")]
    [InlineData("123456")]
    internal void Validate_GivenNonValidLengthStringAndName_ThrowsException(string value)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        var sut = new NonEmptyStringLengthValidator(3, 5);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myString', the value '{value}' does not have a valid length (must be between 3 and 5).");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345")]
    [InlineData("   1")]
    [InlineData("\t\ta\t")]
    internal void Validate_GivenValidString_ReturnsValue(string value)
    {
        var untrusted = new UntrustedValue<string>(value);
        var sut = new NonEmptyStringLengthValidator(3, 5);
        sut.Validate(untrusted).Should().Be(value);
    }
}