using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert;

public class TestMisc(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseAdvertTest(agent, output)
{
    [Fact]
    public async Task CreateAdvertFlatWithApartmentNumber_Success()
    {
        // Arrange
        var advert = BaseAdvertFor(AdvertTypeEnum.Flat) with { ApartmentNumber = 1 };

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.Equal(1, created.Advert.ApartmentNumber);
    }
    
    [Fact]
    public async Task CreateAdvertNonFlatWithApartmentNumber_Fails()
    {
        // Arrange
        var advert = BaseAdvertFor(AdvertTypeEnum.House) with { ApartmentNumber = 1 };

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(responseString);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("ApartmentNumber", responseString);
    }
    
    
}
