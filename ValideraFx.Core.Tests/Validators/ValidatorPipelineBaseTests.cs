// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: GPL-3.0-or-later

using FluentAssertions;
using ValideraFx.Core.Validators;

namespace ValideraFx.Core.Tests.Validators;

public class ValidatorPipelineBaseTests
{
    [Fact]
    internal void Ctor_GivenNoValidators_ThrowsException()
    {
        var constructing = () => new TestPipeline();
        constructing.Should().Throw<ArgumentException>();
    }

    private class TestPipeline() : ValidatorPipelineBase<object>(Array.Empty<ValidatorBase<object>>());
}
