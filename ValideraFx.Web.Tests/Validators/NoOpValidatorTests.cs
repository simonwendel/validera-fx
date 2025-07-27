// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using AutoFixture.Xunit2;
using FluentAssertions;
using ValideraFx.Core;
using ValideraFx.Web.Validators;

namespace ValideraFx.Web.Tests.Validators;

public class NoOpValidatorTests
{
    [Theory, AutoData]
    internal void Validate_Always_ReturnsValue(object obj, NoOpValidator<object> sut)
    {
        var untrusted = new UntrustedValue<object>(obj);
        sut.Validate(untrusted).Should().Be(obj);
    }
}