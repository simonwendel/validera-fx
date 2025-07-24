// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;

namespace ValideraFx.Web.Tests;

public class UntrustedValueBinderProviderTests
{
    [Fact]
    public void GetBinder_GivenNullContext_ThrowsException()
    {
        var provider = new UntrustedValueBinderProvider();
        Action getting = () => provider.GetBinder(null);
        getting.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("context");
    }
}