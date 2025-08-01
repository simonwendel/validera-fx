// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class ElementCountValidatorTests
{
    [Theory]
    [InlineData(10, 9)]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    internal void Ctor_GivenInvalidBounds_ThrowsArgumentException(int minLength, int maxLength)
    {
        Action constructing = () => new ElementCountValidator<IEnumerable<string>>(minLength, maxLength);
        constructing.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(-1, 9)]
    [InlineData(-2, -1)]
    internal void Ctor_GivenNegativeBounds_ThrowsArgumentOutOfRangeException(int minLength, int maxLength)
    {
        Action constructing = () => new ElementCountValidator<IEnumerable<string>>(minLength, maxLength);
        constructing.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    internal void Validate_GivenTooFewElements_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a"]);
        var sut = new ElementCountValidator<string>(2);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The list with 1 element does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenTooFewElementsAndDontRenderValue_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a"]);
        var sut = new ElementCountValidator<string>(2)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenTooFewElementsExactlyZero_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>([]);
        var sut = new ElementCountValidator<string>(2);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The list with 0 elements does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenTooFewElementsExactlyZeroAndDontRenderValue_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>([]);
        var sut = new ElementCountValidator<string>(2)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenNullEnumerable_TreatsAsZeroAndThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(null!);
        var sut = new ElementCountValidator<string>(2);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The list with 0 elements does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenNullEnumerableAndDontRenderValue_TreatsAsZeroAndThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(null!);
        var sut = new ElementCountValidator<string>(2)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenTooFewElementsAndName_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a"], "myList");
        var sut = new ElementCountValidator<string>(2);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myList'. The list with 1 element does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenTooFewElementsAndNameAndDontRenderValue_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a"], "myList");
        var sut = new ElementCountValidator<string>(2)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myList'. The value does not have a valid length (must be between 2 and int.MaxValue).");
    }

    [Fact]
    internal void Validate_GivenTooManyElements_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a", "b", "c", "d", "e", "f"]);
        var sut = new ElementCountValidator<string>(1, 5);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The list with 6 elements does not have a valid length (must be between 1 and 5).");
    }

    [Fact]
    internal void Validate_GivenTooManyElementsAndDontRenderValue_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a", "b", "c", "d", "e", "f"]);
        var sut = new ElementCountValidator<string>(1, 5)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed. The value does not have a valid length (must be between 1 and 5).");
    }

    [Fact]
    internal void Validate_GivenTooManyElementsAndName_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a", "b", "c", "d", "e", "f"], "myList");
        var sut = new ElementCountValidator<string>(1, 5);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myList'. The list with 6 elements does not have a valid length (must be between 1 and 5).");
    }

    [Fact]
    internal void Validate_GivenTooManyElementsAndNameAndDontRenderValue_ThrowsException()
    {
        var untrusted = new UntrustedValue<IEnumerable<string>>(["a", "b", "c", "d", "e", "f"], "myList");
        var sut = new ElementCountValidator<string>(1, 5)
        {
            RenderValue = false
        };
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myList'. The value does not have a valid length (must be between 1 and 5).");
    }

    [Fact]
    internal void Validate_GivenNumberOfElementsWithinRange_ReturnsValue()
    {
        List<string> expected = ["a", "b", "c", "d", "e"];
        var untrusted = new UntrustedValue<IEnumerable<string>>(expected);
        var sut = new ElementCountValidator<string>(4, 6);
        sut.Validate(untrusted).Should().BeEquivalentTo(expected);
    }
}