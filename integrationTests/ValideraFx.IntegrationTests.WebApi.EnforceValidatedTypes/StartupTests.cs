// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ValideraFx.IntegrationTests.WebApi.EnforceValidatedTypes;

public class StartupTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public void ProgramStart_WhenNonValidatedTypesPresentInActionParameters_ThrowsException()
    {
        const string message = """
                               Action: OkController.Get
                                 Parameter: i (System.Int32) <--
                               """;
        Action starting = () => factory.CreateClient();
        starting.Should().Throw<InvalidOperationException>().Which.Message.Should().Contain(message);
    }
}