// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ValideraFx.Web.Tests.Integrations;

public class CalculateTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    [Theory]
    [InlineData(-11)]
    [InlineData(11)]
    public async Task SumNumbers_GivenInvalidFirstNumber_ReturnsBadRequest(int first)
    {
        var url = $"/calculate/sum?first={first}&second=1";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should()
            .Be($"Validation failed for 'first', the value '{first}' is not within the interval [-10, 10].");
    }

    [Theory]
    [InlineData(-11)]
    [InlineData(11)]
    public async Task SumNumbers_GivenInvalidSecondNumber_ReturnsBadRequest(int second)
    {
        var url = $"/calculate/sum?first=1&second={second}";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        content.Should()
            .Be($"Validation failed for 'second', the value '{second}' is not within the interval [-10, 10].");
    }

    [Theory]
    [InlineData(-10, -10, -20)]
    [InlineData(10, 10, 20)]
    [InlineData(5, 7, 12)]
    public async Task SumNumbers_GivenValidNumbers_ReturnsOk(int first, int second, int expected)
    {
        var url = $"/calculate/sum?first={first}&second={second}";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be(expected.ToString());
    }
}