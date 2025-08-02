// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ValideraFx.Core;

namespace ValideraFx.Web.Tests;

public class ValidatorRegistryTests
{
    [Fact]
    public void Ctor_GivenNullServiceCollection_ThrowsArgumentNullException()
    {
        Action constructing = () => new ValidatorRegistry(null!);
        constructing.Should().Throw<ArgumentNullException>().WithParameterName("services");
    }

    [Fact]
    public void GetValidatorFor_GivenNullType_ThrowsArgumentNullException()
    {
        var sut = new ValidatorRegistry(new ServiceCollection());
        Action getting = () =>
            sut.GetValidatorFor(null!, new ServiceCollection().BuildServiceProvider(), new ValidationOptions());
        getting.Should().Throw<ArgumentNullException>().WithParameterName("type");
    }

    [Fact]
    public void GetValidatorFor_GivenNullServiceProvider_ThrowsArgumentNullException()
    {
        var sut = new ValidatorRegistry(new ServiceCollection());
        Action getting = () => sut.GetValidatorFor(typeof(object), null!, new ValidationOptions());
        getting.Should().Throw<ArgumentNullException>().WithParameterName("serviceProvider");
    }

    [Fact]
    public void GetValidatorFor_GivenNullValidationOptions_ThrowsArgumentNullException()
    {
        var sut = new ValidatorRegistry(new ServiceCollection());
        Action getting = () =>
            sut.GetValidatorFor(typeof(object), new ServiceCollection().BuildServiceProvider(), null!);
        getting.Should().Throw<ArgumentNullException>().WithParameterName("options");
    }

    [Fact]
    public void GetValidatorFor_WhenValidatorNotRegisteredForType_ThrowsArgumentNullException()
    {
        var sut = new ValidatorRegistry(new ServiceCollection());
        Action getting = () =>
            sut.GetValidatorFor(typeof(long), new ServiceCollection().BuildServiceProvider(), new ValidationOptions());
        getting.Should().Throw<InvalidOperationException>()
            .WithMessage("No suitable validator for type System.Int64 found.");
    }

    [Fact]
    public void GetValidatorFor_WhenValidatorsAreRegistered_RetrievesFromProviderAndHonorsOptions()
    {
        var services = new ServiceCollection();
        services.AddSingleton(Validation.Of<int>().Apply(x => x, Limit.AtMost(5)).Build());
        var provider = services.BuildServiceProvider();
        var options = new ValidationOptions
        {
            DontRenderValues = true
        };

        var sut = new ValidatorRegistry(services);
        var validatorObject = sut.GetValidatorFor(typeof(int), provider, options)!;

        var renderValue = validatorObject.GetType().GetProperty("RenderValue")!;
        renderValue.GetValue(validatorObject).Should().Be(false);
    }

    [Fact]
    public void GetValidatorFor_WhenValidatorsAreRegistered_RetrievesFromProvider()
    {
        var services = new ServiceCollection();

        var validator1 = Validation.Of<int>().Apply(x => x, Limit.AtMost(5)).Build();
        services.AddSingleton(validator1);

        var validator2 = Validation.Of<string>().Apply(x => x, Limit.Length(10)).Build();
        services.AddTransient(_ => validator2);

        var validator3 = Validation.Of<SomeType>().Apply(x => x.Message, Limit.Length(10)).Build();
        services.AddScoped<IValidator<SomeType>>(_ => validator3);

        var sut = new ValidatorRegistry(services);
        var provider = services.BuildServiceProvider();

        sut.GetValidatorFor(typeof(int), provider, new ValidationOptions())!.GetType().Should()
            .Be(validator1.GetType());
        sut.GetValidatorFor(typeof(string), provider, new ValidationOptions())!.GetType().Should()
            .Be(validator2.GetType());
        sut.GetValidatorFor(typeof(SomeType), provider, new ValidationOptions())!.GetType().Should()
            .Be(validator3.GetType());
    }

    // ReSharper disable once MemberCanBePrivate.Global
    [ExcludeFromCodeCoverage]
    public class SomeType
    {
        public string Message { get; set; } = null!;
    }
}