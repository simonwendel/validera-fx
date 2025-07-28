using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class NotNullObjectValidatorTests
{
    [Theory, AutoData]
    internal void Validate_GivenNull_ThrowsException(NotNullObjectValidator<object> sut)
    {
        var untrusted = new UntrustedValue<object>(null!);
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed. The value must not be null.");
    }
    
    [Theory, AutoData]
    internal void Validate_GivenNullAndName_ThrowsException(NotNullObjectValidator<object> sut)
    {
        var untrusted = new UntrustedValue<object>(null!, "myObject");
        Action validating = () => sut.Validate(untrusted);
        validating.Should()
            .Throw<ValidationException>()
            .WithMessage($"Validation failed for 'myObject'. The value must not be null.");
    }

    [Theory, AutoData]
    internal void Validate_GivenNonNull_ReturnsValue(object value, NotNullObjectValidator<object> sut)
    {
        var untrusted = new UntrustedValue<object>(value);
        sut.Validate(untrusted).Should().Be(value);
    }
}