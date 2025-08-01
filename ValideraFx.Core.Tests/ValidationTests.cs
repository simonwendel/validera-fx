// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests;

public class ValidationTests
{
    [Fact]
    public void Apply_GivenSimpleNonMemberSelectorLambda_ThrowsException()
    {
        Action applying = () => Validation.Of<SomeType>()
            .Apply(x => x.Value1 + 1, Limit.AtLeast(0));

        applying.Should().Throw<ArgumentException>()
            .WithMessage(
                "The selector must be a member access or a bare parameter, but the expression is 'x.Value1 + 1'.");
    }
    [Fact]
    
    public void Apply_GivenComplexNonMemberSelectorLambda_ThrowsException()
    {
        Action applying = () => Validation.Of<SomeType>()
            .Apply(x => x.Value1 + (x.value3 - 1), Limit.AtLeast(0));

        applying.Should().Throw<ArgumentException>()
            .WithMessage(
                "The selector must be a member access or a bare parameter, but the expression is 'x.Value1 + (x.value3 - 1)'.");
    }

    [Fact]
    public void Apply_GivenBareParameterForComplexType_ShouldNotThrow()
    {
        Action applying = () => Validation.Of<SomeType>().Apply(x => x, new SomeTypeCustomValidator());
        applying.Should().NotThrow();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Build_WhenNoValidatorsAdded_ThrowsException(bool allowMissing)
    {
        Action building = () => Validation.Of<SomeType>().Build(allowMissing);
        building.Should().Throw<InvalidOperationException>()
            .WithMessage("No validators have been added to the validation builder.");
    }

    [Fact]
    public void Build_GivenAllowMissingFalseAndMissingProperty_ThrowsException()
    {
        Action building = () => Validation.Of<SomeType>()
            .Apply(x => x.Value1, Limit.AtLeast(0))
            .Build(allowMissing: false);

        building.Should().Throw<InvalidOperationException>()
            .WithMessage("Not all properties or fields of SomeType are validated. Validation is missing for 'Value2, value3'.");
    }

    [Fact]
    public void Build_GivenAllowMissingFalseAndNoMissingProperties_ReturnsValidator()
    {
        var validator = Validation.Of<SomeType>()
            .Apply(x => x.Value1, Limit.AtLeast(0))
            .Apply(x => x.Value2, Limit.AtLeast(0))
            .Apply(x => x.value3, Limit.AtLeast(0))
            .Build(allowMissing: false);

        validator.Should().NotBeNull();
        validator.Should().BeOfType<Pipeline<SomeType>>();
    }

    [Fact]
    public void Build_GivenAllowMissingFalseBareParameterAndMissingProperty_ReturnsValidator()
    {
        var validator = Validation.Of<SomeType>()
            .Apply(x => x.Value1, Limit.AtLeast(0))
            .Apply(x => x, new SomeTypeCustomValidator())
            .Build(allowMissing: false);

        validator.Should().NotBeNull();
        validator.Should().BeOfType<Pipeline<SomeType>>();
    }

    [Fact]
    public void Build_GivenAllowMissingTrueAndMissingProperty_ReturnsValidator()
    {
        var validator = Validation.Of<SomeType>()
            .Apply(x => x.Value1, Limit.AtLeast(0))
            .Build(allowMissing: true);

        validator.Should().NotBeNull();
        validator.Should().BeOfType<Pipeline<SomeType>>();
    }

    [ExcludeFromCodeCoverage]
    private class SomeTypeCustomValidator : IValidator<SomeType>
    {
        public bool RenderValue { get; set; } = true;
        public SomeType Validate(UntrustedValue<SomeType> value) => value.Value;
    }

    [ExcludeFromCodeCoverage]
    private class SomeType
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int value3;

        public void SetValue3(int value)
        {
            value3 = value;
        }
    }
}