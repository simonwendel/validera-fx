// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ValideraFx.Core;
using ValideraFx.Web.ModelBinding;

namespace ValideraFx.Web.Tests.ModelBinding;

public class UntrustedValueValidatorTests
{
    private readonly UntrustedValueValidator sut = new();

    [Fact]
    internal void Validate_GivenUntrustedValueContext_ReturnsEmptyResults()
    {
        var results = sut.Validate(GetContextFor<UntrustedValue<object>>());
        results.Should().BeEmpty();
    }

    [Fact]
    internal void Validate_GivenNonUntrustedValueContext_ReturnsEmptyResults()
    {
        var results = sut.Validate(GetContextFor<object>());
        results.Should().BeEmpty();
    }

    private static ModelValidationContext GetContextFor<T>()
    {
        var metadataProvider = new DefaultModelMetadataProvider(
            new DefaultCompositeMetadataDetailsProvider(
                [
                    new DefaultBindingMetadataProvider(),
                    new DefaultValidationMetadataProvider()
                ]
            ));

        var metadata = metadataProvider.GetMetadataForType(typeof(UntrustedValue<string>));

        return new ModelValidationContext(
            actionContext: new ActionContext(),
            metadata,
            metadataProvider,
            container: null,
            model: new object()
        );
    }
}