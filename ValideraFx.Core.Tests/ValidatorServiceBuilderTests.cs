using System.Reflection;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests;

public class ValidatorServiceBuilderTests
{
    private readonly ValidatorServiceBuilder sut = new();

    [Fact]
    public void AddValidatorOfTOfTValidator_GivenDuplicateValidator_ThrowsException()
    {
        sut.AddValidator<string, NonEmptyStringValidator>();
        Action action = () => sut.AddValidator<string, NonEmptyStringValidator>();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("A type validator for type System.String is already registered.");
    }

    [Fact]
    internal void AddValidatorOfT_GivenNullValidator_ThrowsException()
    {
        Action action = () => sut.AddValidator<object>(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddValidatorOfT_GivenDuplicateValidator_ThrowsException()
    {
        sut.AddValidator(new NonEmptyStringValidator());
        Action action = () => sut.AddValidator(new NonEmptyStringValidator());
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("A type validator for type System.String is already registered.");
    }
    
    [Fact]
    public void Build_GivenNoValidators_ThrowsException()
    {
        Action action = () => sut.Build();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("No validators registered.");
    }
    
    [Fact]
    public void Build_WhenInternalStateIsMessedUp_ThrowsException()
    {
        // This is a bit of a hack, but it tests the robustness of the internal state.
        var field = typeof(ValidatorServiceBuilder).GetField("validators", BindingFlags.NonPublic | BindingFlags.Instance);
        var validators = (Dictionary<Type, object>)field!.GetValue(sut)!;
        validators[typeof(string)] = new object();
        var product = sut.Build();
        Action action = () => product.Validate(new UntrustedValue<string>("test"));
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Validator for type System.String is not of the correct type.");
    }
    
    [Fact]
    public void Build_WhenValidatorsRegistered_ReturnsValidator()
    {
        var product = sut.AddValidator<string, NonEmptyStringValidator>().AddValidator(new IntegerIntervalValidator(0, 10)).Build();
        product.Should().NotBeNull();
        
        var untrustedString = new UntrustedValue<string>("test");
        var untrustedInteger = new UntrustedValue<int>(5);
        
        product.Validate(untrustedString).Should().Be("test");
        product.Validate(untrustedInteger).Should().Be(5);

        Action validatingString = () => product.Validate(new UntrustedValue<string>(""));
        validatingString.Should().Throw<ValidationException>();
        
        Action validatingInteger = () => product.Validate(new UntrustedValue<int>(-1));
        validatingInteger.Should().Throw<ValidationException>();
        
        Action validatingNonRegisteredType = () => product.Validate(new UntrustedValue<double>(3.14));
        validatingNonRegisteredType.Should().Throw<InvalidOperationException>()
            .WithMessage("No validator registered for type System.Double.");
    }
}