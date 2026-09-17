using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Shared.SRealty.Advert.Enums.Extensions;
using Server.Tests.Srealty;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert;

public class TestAdvertSubtypes(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseAdvertTest(agent, output)
{


    [Theory]
    [MemberData(nameof(SubtypesOutOfRangeByType))]
    public async Task CreateAdvert_WithAnOutOfRangeSubtype_Fails(AdvertTypeEnum type, AdvertSubtypeEnum subtype)
    {
        // Arrange
        var advert = BaseAdvertFor(type) with { AdvertSubtype = subtype };

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(responseString);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(responseString);
        Assert.Contains("AdvertSubtype", responseString);
    }

    [Theory]
    [MemberData(nameof(NotValidSubtypesByType))]
    public async Task CreateAdvert_WithAnInvalidSubtype_Fails(AdvertTypeEnum type, AdvertSubtypeEnum subtype)
    {
        // Arrange
        var advert = BaseAdvertFor(type) with { AdvertSubtype = subtype };

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(responseString);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(responseString);
        Assert.Contains("AdvertSubtype", responseString);
    }

    [Theory]
    [MemberData(nameof(SubtypesByType))]
    public async Task CreateAdvert_WithASubtypeValidForItsType_Succeeds(AdvertTypeEnum type, AdvertSubtypeEnum subtype)
    {
        // Arrange
        var advert = BaseAdvertFor(type) with { AdvertSubtype = subtype };

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();
        var body = created?.Advert;

        // Assert
        output.WriteLine(body?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.AdvertId);
        Assert.Equal(type, body.AdvertType);
        Assert.Equal(subtype, body.AdvertSubtype);
    }
}
