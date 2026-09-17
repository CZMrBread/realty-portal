using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;

namespace Server.Tests.Srealty;

public class TestAdvertFactory(SharedAgentFixture agent) : IClassFixture<SharedAgentFixture>
{
    private string ApiBaseUrl => "api/srealty/advert";
    private readonly AdvertFactory advertFactory = new();

    [Fact]
    public async Task CreateFlatAdvert_ReturnsValidAdvert()
    {
        // Arrange
        var advert = advertFactory.CreateFlatAdvert(agent.UserId);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();
        var body = created?.Advert;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.AdvertId);
        Assert.Equal(AdvertTypeEnum.Flat, body.AdvertType);
    }

    [Fact]
    public async Task CreateHouseAdvert_ReturnsValidAdvert()
    {
        // Arrange
        var advert = advertFactory.CreateHouseAdvert(agent.UserId);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();
        var body = created?.Advert;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.AdvertId);
        Assert.Equal(AdvertTypeEnum.House, body.AdvertType);
    }

    [Fact]
    public async Task CreateLandAdvert_ReturnsValidAdvert()
    {
        // Arrange
        var advert = advertFactory.CreateLandAdvert(agent.UserId);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();
        var body = created?.Advert;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.AdvertId);
        Assert.Equal(AdvertTypeEnum.Land, body.AdvertType);
    }

    [Fact]
    public async Task CreateCommercialAdvert_ReturnsValidAdvert()
    {
        // Arrange
        var advert = advertFactory.CreateCommercialAdvert(agent.UserId);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();
        var body = created?.Advert;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.AdvertId);
        Assert.Equal(AdvertTypeEnum.Commercial, body.AdvertType);
    }

    [Fact]
    public async Task CreateOtherAdvert_ReturnsValidAdvert()
    {
        // Arrange
        var advert = advertFactory.CreateOtherAdvert(agent.UserId);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();
        var body = created?.Advert;

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.AdvertId);
        Assert.Equal(AdvertTypeEnum.Other, body.AdvertType);
    }
}
