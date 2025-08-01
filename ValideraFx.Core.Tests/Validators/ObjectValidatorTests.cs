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
        var (sut, obj) = CreateSystemFor(string.Empty);
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage("Validation failed for 'TestProperty'. The value is empty.");
    }

    [Fact]
    internal void Validate_GivenInvalidPropertyAndName_ThrowsException()
    {
        var (sut, obj) = CreateSystemFor(string.Empty);
        Action validating = () => sut.Validate(new UntrustedValue<TestValue>(obj, "myObject"));
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage("Validation failed for 'myObject.TestProperty'. The value is empty.");
    }

    [Theory, AutoData]
    internal void Validate_GivenValidProperty_ReturnsValue(string value)
    {
        var (sut, obj) = CreateSystemFor(value);
        sut.Validate(new UntrustedValue<TestValue>(obj)).TestProperty.Should().Be(value);
    }

    private class TestValue(string testProperty)
    {
        public string TestProperty { get; } = testProperty;
    }

    private (ObjectValidator<TestValue, string>, TestValue) CreateSystemFor(string value)
    {
        var validator = new NonEmptyStringValidator();
        var obj = new TestValue(value);
        var sut = new ObjectValidator<TestValue, string>(x => x.TestProperty, validator);
        return (sut, obj);
    }
}