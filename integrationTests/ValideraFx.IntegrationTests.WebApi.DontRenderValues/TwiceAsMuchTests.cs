// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ValideraFx.IntegrationTests.WebApi.DontRenderValues;

public class TwiceAsMuchTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    [Theory]
    [InlineData(-1)]
    [InlineData(30001)]
    public async Task Get_GivenInvalidNumber_ReturnsBadRequestWithoutRenderingValue(int number)
    {
        var url = $"/twiceAsMuch?number={number}";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Errors["number"][0].Should()
            .Be("The value is not within the interval [0, 30000].");
    }

    [Fact]
    public async Task Get_GivenValidNumber_ReturnsOk()
    {
        const string url = "/twiceAsMuch?number=256";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var answer = int.Parse(content);
        answer.Should().Be(512);
    }
}