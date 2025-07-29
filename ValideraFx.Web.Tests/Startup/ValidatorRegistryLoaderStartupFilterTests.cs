// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Moq;
using ValideraFx.Web.Startup;

namespace ValideraFx.Web.Tests.Startup;

public class ValidatorRegistryLoaderStartupFilterTests
{
    private readonly Mock<IValidatorRegistry> registry;
    private readonly ValidatorRegistryLoaderStartupFilter sut;

    public ValidatorRegistryLoaderStartupFilterTests()
    {
        registry = new Mock<IValidatorRegistry>();
        sut = new ValidatorRegistryLoaderStartupFilter(registry.Object);
    }
    
    [Fact]
    public void Configure_GivenNullAction_ThrowsException()
    {
        Action configuring = () => sut.Configure(null!);
        configuring.Should()
            .Throw<ArgumentNullException>().WithParameterName("next");
        registry.Verify(r => r.Load(), Times.Never);
    }
    
    [Fact]
    public void Configure_GivenAction_LoadsValidatorRegistry()
    {
        var called = false;
        Action<IApplicationBuilder> next = (_) => called = true;
        sut.Configure(next).Invoke(Mock.Of<IApplicationBuilder>());
        called.Should().BeTrue();
        registry.Verify(r => r.Load(), Times.Once);
    }
}