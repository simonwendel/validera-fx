// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;

namespace ValideraFx.Web.Tests;

public class ValidationOptionsTests
{
    [Theory]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void StrictValidationModeGetter_WhenEitherOtherPropertyIsFalse_ReturnsFalse(
        bool enforceValidModelState,
        bool enforceValidatedTypes,
        bool dontRenderValues)
    {
        var options = new ValidationOptions
        {
            EnforceValidModelState = enforceValidModelState,
            EnforceValidatedTypes = enforceValidatedTypes,
            DontRenderValues = dontRenderValues,
        };
        
        options.StrictValidationMode.Should().BeFalse();
    }
    
    [Fact]
    public void StrictValidationModeGetter_WhenOtherPropertiesAreTrue_ReturnsTrue()
    {
        var options = new ValidationOptions
        {
            EnforceValidModelState = true,
            EnforceValidatedTypes = true,
            DontRenderValues = true,
        };
        
        options.StrictValidationMode.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void StrictValidationModeSetter_GivenFalse_ChangesNothing(
        bool enforceValidModelState,
        bool enforceValidatedTypes,
        bool dontRenderValues)
    {
        var options = new ValidationOptions
        {
            EnforceValidModelState = enforceValidModelState,
            EnforceValidatedTypes = enforceValidatedTypes,
            DontRenderValues = dontRenderValues,
            StrictValidationMode = false
        };

        options.EnforceValidModelState.Should().Be(enforceValidModelState);
        options.EnforceValidatedTypes.Should().Be(enforceValidatedTypes);
        options.DontRenderValues.Should().Be(dontRenderValues);
    }
    
    [Fact]
    public void StrictValidationModeSetter_GivenTrue_SetsOtherPropertiesToTrue()
    {
        var options = new ValidationOptions
        {
            StrictValidationMode = true
        };

        options.StrictValidationMode.Should().BeTrue();
        options.EnforceValidModelState.Should().BeTrue();
        options.EnforceValidatedTypes.Should().BeTrue();
        options.DontRenderValues.Should().BeTrue();
    }
}