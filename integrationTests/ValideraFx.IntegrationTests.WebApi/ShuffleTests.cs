// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ValideraFx.IntegrationTests.WebApi;

public class ShuffleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client = factory.CreateClient();

    [Fact]
    public async Task GetShuffle_GivenZeroItems_ReturnsBadRequest()
    {
        const string url = "/shuffle";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Errors["options.Items"][0].Should()
            .Be("The list with 0 elements does not have a valid length (must be between 3 and 7).");
    }

    [Theory]
    [InlineData("items=a", "1 element")]
    [InlineData("items=a&items=b", "2 elements")]
    public async Task GetShuffle_GivenTooFewItems_ReturnsBadRequest(string items, string count)
    {
        var url = $"/shuffle?{items}";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Errors["options.Items"][0].Should()
            .Be($"The list with {count} does not have a valid length (must be between 3 and 7).");
    }

    [Fact]
    public async Task GetShuffle_GivenTooManyItems_ReturnsBadRequest()
    {
        const string url = "/shuffle?items=a&items=b&items=c&items=d&items=e&items=f&items=g&items=h";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        problem!.Errors["options.Items"][0].Should()
            .Be("The list with 8 elements does not have a valid length (must be between 3 and 7).");
    }

    [Theory]
    [InlineData(
        "items=a&items=b&items=c", new[] { "a", "b", "c" })]
    [InlineData(
        "items=a&items=b&items=c&items=d&items=e", new[] { "a", "b", "c", "d", "e" })]
    [InlineData(
        "items=a&items=b&items=c&items=d&items=e&items=f&items=g", new[] { "a", "b", "c", "d", "e", "f", "g" })]
    public async Task GetMessage_GivenValidOptions_ReturnsOk(string items, string[] expected)
    {
        var url = $"/shuffle?{items}";
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IList<string>>();
        content.Should().BeEquivalentTo(expected);
    }
}