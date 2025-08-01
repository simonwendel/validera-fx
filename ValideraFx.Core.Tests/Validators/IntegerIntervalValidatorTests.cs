// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class IntegerIntervalValidatorTests
{
    [Theory]
    [InlineData(10, 9)]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    internal void Ctor_GivenInvalidBounds_ThrowsArgumentException(int lowerBounds, int upperBounds)
    {
        Action constructing = () => new IntegerIntervalValidator(lowerBounds, upperBounds);
        constructing.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineAutoData(0, 1)]
    [InlineAutoData(3, 4)]
    [InlineAutoData(5, 6)]
    internal void Validate_GivenValueLessThanLowerBounds_ThrowsException(int value, int lowerBounds)
    {
        var untrusted = new UntrustedValue<int>(value);
        var sut = new IntegerIntervalValidator(lowerBounds);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value '{value}' is not within the interval [{lowerBounds}, int.MaxValue].");
    }

    [Theory]
    [InlineAutoData(0, 1)]
    [InlineAutoData(3, 4)]
    [InlineAutoData(5, 6)]
    internal void Validate_GivenValueLessThanLowerBoundsAndDontRenderValue_ThrowsException(int value, int lowerBounds)
    {
        var untrusted = new UntrustedValue<int>(value);
        var sut = new IntegerIntervalValidator(lowerBounds)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value is not within the interval [{lowerBounds}, int.MaxValue].");
    }

    [Theory]
    [InlineAutoData(0, 1)]
    [InlineAutoData(3, 4)]
    [InlineAutoData(5, 6)]
    internal void Validate_GivenValueLessThanLowerBoundsAndName_ThrowsException(int value, int lowerBounds)
    {
        var untrusted = new UntrustedValue<int>(value, "myInteger");
        var sut = new IntegerIntervalValidator(lowerBounds);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myInteger'. The value '{value}' is not within the interval [{lowerBounds}, int.MaxValue].");
    }

    [Theory]
    [InlineAutoData(0, 1)]
    [InlineAutoData(3, 4)]
    [InlineAutoData(5, 6)]
    internal void Validate_GivenValueLessThanLowerBoundsAndNameAndDontRenderValue_ThrowsException(
        int value,
        int lowerBounds)
    {
        var untrusted = new UntrustedValue<int>(value, "myInteger");
        var sut = new IntegerIntervalValidator(lowerBounds)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myInteger'. The value is not within the interval [{lowerBounds}, int.MaxValue].");
    }

    [Theory]
    [InlineAutoData(2, 0, 1)]
    [InlineAutoData(5, 2, 4)]
    [InlineAutoData(21, 6, 20)]
    internal void Validate_GivenValueGreaterThenUpperBounds_ThrowsException(int value, int lowerBounds, int upperBounds)
    {
        var untrusted = new UntrustedValue<int>(value);
        var sut = new IntegerIntervalValidator(lowerBounds, upperBounds);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value '{value}' is not within the interval [{lowerBounds}, {upperBounds}].");
    }

    [Theory]
    [InlineAutoData(2, 0, 1)]
    [InlineAutoData(5, 2, 4)]
    [InlineAutoData(21, 6, 20)]
    internal void Validate_GivenValueGreaterThenUpperBoundsAndDontRenderValue_ThrowsException(
        int value,
        int lowerBounds, 
        int upperBounds)
    {
        var untrusted = new UntrustedValue<int>(value);
        var sut = new IntegerIntervalValidator(lowerBounds, upperBounds)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value is not within the interval [{lowerBounds}, {upperBounds}].");
    }

    [Theory]
    [InlineAutoData(2, 0, 1)]
    [InlineAutoData(5, 2, 4)]
    [InlineAutoData(21, 6, 20)]
    internal void Validate_GivenValueGreaterThenUpperBoundsAndName_ThrowsException(
        int value, 
        int lowerBounds,
        int upperBounds)
    {
        var untrusted = new UntrustedValue<int>(value, "myInteger");
        var sut = new IntegerIntervalValidator(lowerBounds, upperBounds);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myInteger'. The value '{value}' is not within the interval [{lowerBounds}, {upperBounds}].");
    }

    [Theory]
    [InlineAutoData(2, 0, 1)]
    [InlineAutoData(5, 2, 4)]
    [InlineAutoData(21, 6, 20)]
    internal void Validate_GivenValueGreaterThenUpperBoundsAndNameAndDontRenderValue_ThrowsException(
        int value,
        int lowerBounds,
        int upperBounds)
    {
        var untrusted = new UntrustedValue<int>(value, "myInteger");
        var sut = new IntegerIntervalValidator(lowerBounds, upperBounds)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myInteger'. The value is not within the interval [{lowerBounds}, {upperBounds}].");
    }

    [Fact]
    internal void Validate_GivenValueWithinBounds_ReturnsValue()
    {
        var expected = 5;
        var untrusted = new UntrustedValue<int>(expected);
        var sut = new IntegerIntervalValidator(4, 6);
        sut.Validate(untrusted).Should().Be(expected);
    }
}