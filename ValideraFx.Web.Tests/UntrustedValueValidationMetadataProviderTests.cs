// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using ValideraFx.Core;

namespace ValideraFx.Web.Tests;

public class UntrustedValueValidationMetadataProviderTests
{
    private readonly UntrustedValueValidationMetadataProvider sut = new();

    [Fact]
    public void CreateValidationMetadata_GivenUntrustedValue_SetsValidateChildrenToFalse()
    {
        var context = GetContextFor<UntrustedValue<object>>();
        sut.CreateValidationMetadata(context);
        context.ValidationMetadata.ValidateChildren.Should().BeFalse();
    }

    [Fact]
    public void CreateValidationMetadata_GivenNonUntrustedValue_DoesNotSetValidateChildrenToFalse()
    {
        var context = GetContextFor<object>();
        sut.CreateValidationMetadata(context);
        context.ValidationMetadata.ValidateChildren.Should().BeNull();
    }

    private static ValidationMetadataProviderContext GetContextFor<T>() =>
        new(ModelMetadataIdentity.ForType(typeof(T)), ModelAttributes.GetAttributesForType(typeof(T)));
}