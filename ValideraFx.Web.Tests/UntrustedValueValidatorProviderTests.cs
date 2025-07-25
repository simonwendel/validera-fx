// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using ValideraFx.Core;

namespace ValideraFx.Web.Tests;

public class UntrustedValueValidatorProviderTests
{
    private readonly UntrustedValueValidatorProvider sut = new();
    private readonly ModelValidatorProviderContext stringContext;
    private readonly ModelValidatorProviderContext untrustedValueContext;

    public UntrustedValueValidatorProviderTests()
    {
        List<ValidatorItem> validatorList =
        [
            new() { Validator = Mock.Of<IModelValidator>(), IsReusable = true },
            new() { Validator = Mock.Of<IModelValidator>(), IsReusable = true }
        ];

        var untrustedValueMetadata = GetMetadataFor<UntrustedValue<string>>();
        untrustedValueContext = new ModelValidatorProviderContext(untrustedValueMetadata, validatorList);
        var stringMetadata = GetMetadataFor<string>();
        stringContext = new ModelValidatorProviderContext(stringMetadata, validatorList);
    }

    [Fact]
    public void CreateValidators_WithUntrustedValueType_ReplacesAllWithUntrustedValueValidator()
    {
        sut.CreateValidators(untrustedValueContext);
        untrustedValueContext.Results.Should()
            .ContainSingle()
            .Which.Validator.Should().BeOfType<UntrustedValueValidator>();
        untrustedValueContext.Results.First().IsReusable.Should().BeTrue();
    }

    [Fact]
    public void CreateValidators_WithNonUntrustedValueType_DoesNotReplaceValidators()
    {
        sut.CreateValidators(stringContext);
        stringContext.Results.Should().HaveCount(2);
        stringContext.Results.Should().AllSatisfy(x => x.Validator.Should().NotBeOfType<UntrustedValueValidator>());
    }

    private static ModelMetadata GetMetadataFor<T>() => GetDefaultModelMetadataProvider().GetMetadataForType(typeof(T));

    private static DefaultModelMetadataProvider GetDefaultModelMetadataProvider() => new(
        new DefaultCompositeMetadataDetailsProvider([
            new DefaultBindingMetadataProvider(),
            new DefaultValidationMetadataProvider()
        ])
    );
}