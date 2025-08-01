// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class NonEmptyStringValidatorTests
{
    [Theory]
    [InlineAutoData(null, true)]
    [InlineAutoData(null, false)]
    internal void Validate_GivenNullString_ThrowsExceptionAndIgnoresRenderValue(
        string value, 
        bool renderValue,
        NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        sut.RenderValue = renderValue;
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value is null.");
    }

    [Theory]
    [InlineAutoData("", true)]
    [InlineAutoData("", false)]
    [InlineAutoData(" ", true)]
    [InlineAutoData(" ", false)]
    internal void Validate_GivenEmptyString_ThrowsExceptionAndIgnoresRenderValue(
        string value, 
        bool renderValue,
        NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        sut.RenderValue = renderValue;
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value is empty.");
    }

    [Theory]
    [InlineAutoData(null, true)]
    [InlineAutoData(null, false)]
    internal void Validate_GivenNullStringAndName_ThrowsExceptionAndIgnoresRenderValue(
        string value, 
        bool renderValue,
        NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        sut.RenderValue = renderValue;
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myString'. The value is null.");
    }

    [Theory]
    [InlineAutoData("", true)]
    [InlineAutoData("", false)]
    [InlineAutoData(" ", true)]
    [InlineAutoData(" ", false)]
    internal void Validate_GivenEmptyStringAndName_ThrowsExceptionAndIgnoresRenderValue(
        string value, 
        bool renderValue,
        NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value, "myString");
        sut.RenderValue = renderValue;
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myString'. The value is empty.");
    }

    [Theory, AutoData]
    internal void Validate_GivenStringWithContents_ReturnsValue(string value, NonEmptyStringValidator sut)
    {
        var untrusted = new UntrustedValue<string>(value);
        sut.Validate(untrusted).Should().Be(value);
    }
}