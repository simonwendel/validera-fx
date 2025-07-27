// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using ValideraFx.Core;
using ValideraFx.Web.ModelBinding;

namespace ValideraFx.Web.Tests.ModelBinding;

public class ModelBinderProviderTests
{
    private readonly ModelBinderProvider sut = new();

    [Fact]
    public void GetBinder_GivenNullContext_ThrowsException()
    {
        Action getting = () => sut.GetBinder(null);
        getting.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("context");
    }

    [Fact]
    public void GetBinder_GivenContextNotWithUntrustedValueOrTrustedValue_ReturnsNull()
    {
        var context = GetModelBinderProviderContext<string>();
        var binder = sut.GetBinder(context);
        binder.Should().BeNull();
    }

    [Fact]
    public void GetBinder_GivenContextWithUntrustedValueBinder_ReturnsValueBinder()
    {
        var context = GetModelBinderProviderContext<UntrustedValue<string>>();
        var binder = sut.GetBinder(context);
        binder.Should().BeOfType<UntrustedModelBinder>();
    }

    [Fact]
    public void GetBinder_GivenContextWithTrustedValueBinder_ReturnsValueBinder()
    {
        var context = GetModelBinderProviderContext<TrustedValue<string>>();
        var binder = sut.GetBinder(context);
        binder.Should().BeOfType<TrustedModelBinder>();
    }
    
    private static ModelBinderProviderContext GetModelBinderProviderContext<T>()
    {
        var mockContext = new Mock<ModelBinderProviderContext>();
        mockContext.SetupGet(c => c.Metadata).Returns(
            new DefaultModelMetadataProvider(
                    new DefaultCompositeMetadataDetailsProvider(
                    [
                        new DefaultBindingMetadataProvider(),
                        new DefaultValidationMetadataProvider()
                    ]))
                .GetMetadataForType(typeof(T))
        );
        var mockServices = new Mock<IServiceProvider>();
        mockServices.Setup(x => x.GetService(typeof(IModelMetadataProvider)))
            .Returns(Mock.Of<IModelMetadataProvider>());
        mockServices.Setup(x => x.GetService(typeof(IModelBinderFactory)))
            .Returns(Mock.Of<IModelBinderFactory>());

        mockContext.Setup(x => x.Services).Returns(mockServices.Object);
        return mockContext.Object;
    }
}