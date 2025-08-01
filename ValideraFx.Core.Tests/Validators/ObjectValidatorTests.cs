// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class ObjectValidatorTests
{
    [Fact]
    internal void Validate_GivenInvalidProperty_ThrowsException()
    {
        var (sut, obj) = CreateSystemFor("123456");
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'TestProperty'. The value '123456' does not have a valid length (must be between 0 and 5).");
    }

    [Fact]
    internal void Validate_GivenInvalidEmptyProperty_ThrowsException()
    {
        var (sut, obj) = CreateSystemFor(string.Empty);
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage("Validation failed for 'TestProperty'. The value is empty.");
    }

    [Fact]
    internal void Validate_GivenInvalidPropertyAndDontRenderValue_ThrowsException()
    {
        var (sut, obj) = CreateSystemFor("123456");
        sut.RenderValue = false;
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'TestProperty'. The value does not have a valid length (must be between 0 and 5).");
    }

    [Fact]
    internal void Validate_GivenInvalidPropertyAndName_ThrowsException()
    {
        var (sut, obj) = CreateSystemFor("123456");
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj, "myObject"));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myObject.TestProperty'. The value '123456' does not have a valid length (must be between 0 and 5).");
    }

    [Fact]
    internal void Validate_GivenInvalidEmptyPropertyAndName_ThrowsException()
    {
        var (sut, obj) = CreateSystemFor(string.Empty);
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj, "myObject"));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage("Validation failed for 'myObject.TestProperty'. The value is empty.");
    }

    [Fact]
    internal void Validate_GivenInvalidPropertyAndNameAndDontRenderProperty_ThrowsException()
    {
        var (sut, obj) = CreateSystemFor("123456");
        sut.RenderValue = false;
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj, "myObject"));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Validation failed for 'myObject.TestProperty'. The value does not have a valid length (must be between 0 and 5).");
    }

    [Fact]
    internal void Validate_GivenValidProperty_ReturnsValue()
    {
        var (sut, obj) = CreateSystemFor("valid");
        sut.Validate(new UntrustedValue<TestValue>(obj)).TestProperty.Should().Be("valid");
    }

    private class TestValue(string testProperty)
    {
        public string TestProperty { get; } = testProperty;
    }

    private (ObjectValidator<TestValue, string>, TestValue) CreateSystemFor(string value)
    {
        var validator = new NonEmptyStringLengthValidator(0, 5);
        var obj = new TestValue(value);
        var sut = new ObjectValidator<TestValue, string>(x => x.TestProperty, validator);
        return (sut, obj);
    }
}