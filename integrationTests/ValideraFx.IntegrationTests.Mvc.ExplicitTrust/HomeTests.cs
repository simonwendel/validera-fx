// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ValideraFx.IntegrationTests.Mvc.ExplicitTrust;

public class HomeTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();
    
    [Theory]
    [InlineData("123")]
    [InlineData("1234567890")]
    public async Task GetMessage_GivenValidLengthMessage_ReturnsOkAndMessage(string message)
    {
        var response = await client.GetAsync($"/Message/{message}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain($"<h1>{message}</h1>");
    }
    
    [Theory]
    [InlineData("12")]
    [InlineData("12345678901")]
    public async Task GetMessage_GivenNonValidLengthMessage_ReturnsBadRequest(string message)
    {
        var response = await client.GetAsync($"/Message/{message}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Errors["message"][0].Should()
            .Be($"The value '{message}' does not have a valid length (must be between 3 and 10).");
    }
}