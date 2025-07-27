// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Moq;

namespace ValideraFx.Core.Tests;

public class TrustedValueTests
{
    private const int Value = 123;
    private const string Name = "name";
    private readonly UntrustedValue<int> untrustedValue = new(Value, Name);
    private readonly Mock<IValidator<int>> validator;
    private readonly TrustedValue<int> sut;

    public TrustedValueTests()
    {
        validator = new Mock<IValidator<int>>();
        validator.Setup(v => v.Validate(untrustedValue)).Returns(Value);
        sut = new TrustedValue<int>(untrustedValue, validator.Object);
    }

    [Fact]
    public void Ctor_WhenValidatorIsNull_ThrowsArgumentNullException()
    {
        Action action = () => new TrustedValue<int>(untrustedValue, null!);
        action.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().Be("validator");
    }

    [Fact]
    public void Ctor_WhenUntrustedValueIsNull_ThrowsArgumentNullException()
    {
        Action action = () => new TrustedValue<int>(null!, new Mock<IValidator<int>>().Object);
        action.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().Be("value");
    }

    [Fact]
    public void Ctor_WhenValidationFails_ThrowsValidationException()
    {
        var exception = new ValidationException("name", "message");
        validator.Setup(x => x.Validate(untrustedValue)).Throws(exception);
        Action action = () => new TrustedValue<int>(untrustedValue, validator.Object);
        action.Should().ThrowExactly<ValidationException>().Which.Should().Be(exception);
    }

    [Fact]
    public void ValueGet_Always_ReturnsValue() =>
        sut.Value.Should().Be(Value);

    [Fact]
    public void NameGet_Always_ReturnsValueName() =>
        sut.Name.Should().Be(Name);


    [Fact]
    public void Equals_Always_ReturnsValueEquals()
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        sut.Equals(Value).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_Always_ReturnsValueGetHashCode() =>
        sut.GetHashCode().Should().Be(Value.GetHashCode());

    [Fact]
    public void ToString_Always_ReturnsValueToString() =>
        sut.ToString().Should().Be(Value.ToString());
}