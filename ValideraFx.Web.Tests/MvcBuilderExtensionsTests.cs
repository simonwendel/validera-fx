// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using ValideraFx.Web.ModelBinding;

namespace ValideraFx.Web.Tests;

public class MvcBuilderExtensionsTests
{
    private readonly Mock<IMvcBuilder> builder = new();
    private readonly ServiceCollection services = [];

    [Fact]
    public void AddValideraFx_GivenMvcBuilder_AddsUntrustedValueBinderProvider()
    {
        builder.Setup(m => m.Services).Returns(services);
        builder.Object.AddValideraFx();
        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
        options.ModelBinderProviders[0].Should().BeOfType<UntrustedValueBinderProvider>();
    }
}