// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;

namespace ValideraFx.Web.Tests;

public class DefaultValidationOptionsSetupTests
{
    [Theory, AutoData]
    public void Configure_GivenOptions_SetsEnforceValidModelStateToTrue(DefaultValidationOptionsSetup sut)
    {
        var options = new ValidationOptions();
        sut.Configure(options);
        options.EnforceValidModelState.Should().BeTrue();
    }
    
    [Theory, AutoData]
    public void Configure_GivenOptions_SetsEnforceValidatedTypesToFalse(DefaultValidationOptionsSetup sut)
    {
        var options = new ValidationOptions();
        sut.Configure(options);
        options.EnforceValidatedTypes.Should().BeFalse();
    }
    
    [Theory, AutoData]
    public void Configure_GivenOptions_SetsDontRenderValuesToFalse(DefaultValidationOptionsSetup sut)
    {
        var options = new ValidationOptions();
        sut.Configure(options);
        options.DontRenderValues.Should().BeFalse();
    }
}