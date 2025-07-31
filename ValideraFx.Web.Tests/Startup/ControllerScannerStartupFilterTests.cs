// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Moq;
using ValideraFx.Core;
using ValideraFx.Web.Startup;

namespace ValideraFx.Web.Tests.Startup;

public class ControllerScannerStartupFilterTests
{
    private readonly Dictionary<string, List<(string Name, Type Type)>> controllers = new()
    {
        { "UntrustedController.Get", [("UntrustedValue", typeof(UntrustedValue<string>))] },
        { "TrustedController.Post", [("TrustedValue", typeof(TrustedValue<int>))] },
        {
            "MixedController.Get",
            [("UntrustedValue", typeof(UntrustedValue<string>)), ("TrustedValue", typeof(TrustedValue<int>))]
        }
    };

    private readonly Mock<IMvcControllerScanner> scanner;
    private readonly ControllerScannerStartupFilter sut;

    public ControllerScannerStartupFilterTests()
    {
        scanner = new Mock<IMvcControllerScanner>();
        scanner.Setup(r => r.Scan()).Returns(controllers);
        sut = new ControllerScannerStartupFilter(scanner.Object);
    }

    [Fact]
    public void Configure_GivenNullAction_ThrowsException()
    {
        Action configuring = () => sut.Configure(null!);
        configuring.Should()
            .Throw<ArgumentNullException>().WithParameterName("next");
        scanner.Verify(r => r.Scan(), Times.Never);
    }

    [Fact]
    public void Configure_WhenScannerFindsNonUntrustedNonTrustedValues_ThrowsException()
    {
        var called = false;
        Action<IApplicationBuilder> next = (_) => called = true;
        controllers.Add("StringController", [("StringValue1", typeof(string)), ("StringValue2", typeof(string))]);
        var configuring = () => sut.Configure(next).Invoke(Mock.Of<IApplicationBuilder>());
        configuring.Should().Throw<InvalidOperationException>();
        called.Should().BeFalse();
    }

    [Fact]
    public void Configure_WhenScannerOnlyFindsUntrustedAndTrustedValues_CallsNext()
    {
        var called = false;
        Action<IApplicationBuilder> next = (_) => called = true;
        sut.Configure(next).Invoke(Mock.Of<IApplicationBuilder>());
        called.Should().BeTrue();
        scanner.Verify(r => r.Scan(), Times.Once);
    }
}