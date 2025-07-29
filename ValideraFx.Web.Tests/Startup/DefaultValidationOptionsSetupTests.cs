// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Web.Startup;

namespace ValideraFx.Web.Tests.Startup;

public class DefaultValidationOptionsSetupTests
{
    [Theory, AutoData]
    public void Configure_GivenOptions_SetsEnforceValidModelStateToTrue(DefaultValidationOptionsSetup sut)
    {
        var options = new ValidationOptions();
        sut.Configure(options);
        options.EnforceValidModelState.Should().BeTrue();
    }
}