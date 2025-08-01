// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class NotNullObjectValidatorTests
{
    [Theory]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    internal void Validate_GivenNull_ThrowsExceptionAndIgnoresRenderValue(
        bool renderValue,
        NotNullObjectValidator<object> sut)
    {
        var untrusted = new UntrustedValue<object>(null!);
        sut.RenderValue = renderValue;
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value is null.");
    }

    [Theory]
    [InlineAutoData(true)]
    [InlineAutoData(false)]
    internal void Validate_GivenNullAndName_ThrowsExceptionAndIgnoresRenderValue(
        bool renderValue,
        NotNullObjectValidator<object> sut)
    {
        var untrusted = new UntrustedValue<object>(null!, "myObject");
        sut.RenderValue = renderValue;
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myObject'. The value is null.");
    }

    [Theory, AutoData]
    internal void Validate_GivenNonNull_ReturnsValue(object value, NotNullObjectValidator<object> sut)
    {
        var untrusted = new UntrustedValue<object>(value);
        sut.Validate(untrusted).Should().Be(value);
    }
}