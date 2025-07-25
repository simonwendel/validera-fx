// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ValideraFx.Web.Tests.Integrations;

public class MessageTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    [Theory]
    [InlineData("01")]
    [InlineData("01234567890")]
    public async Task GetMessage_GivenInvalidLengthMessage_ReturnsBadRequest(string message)
    {
        var url = $"/message?message={message}&numberOfTimes=1";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public async Task GetMessage_GivenInvalidNumberOfTimes_ReturnsBadRequest(int numberOfTimes)
    {
        var url = $"/message?message=hello&numberOfTimes={numberOfTimes}";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("hey", 1, "hey")]
    [InlineData("yoyo", 2, "yoyo\nyoyo")]
    [InlineData("hello", 10, "hello\nhello\nhello\nhello\nhello\nhello\nhello\nhello\nhello\nhello")]
    public async Task GetMessage_GivenValidOptions_ReturnsOk(string message, int numberOfTimes, string expected)
    {
        var url = $"/message?message={message}&numberOfTimes={numberOfTimes}";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be(expected);
    }
}