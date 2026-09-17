using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Shares;

public class TestShares(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseAdvertTest(agent, output)
{
    public SrealityAdvertDto BaseShareAdvert(AdvertTypeEnum advertType) =>
        BaseAdvertFor(advertType) with
        {
            AdvertFunction = AdvertFunctionEnum.Shares,
            ShareNumerator = 1,
            ShareDenominator = 1
        };
    
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(int.MaxValue, int.MaxValue)]
    [InlineData(int.MaxValue, 1)]
    [InlineData(1, int.MaxValue)]
    public async Task CreateAdvertAuction_Success(int shareNumerator, int shareDenominator)
    {
        // Arrange
        var advert = BaseShareAdvert(AdvertTypeEnum.Flat) with
        {
            ShareNumerator = shareNumerator,
            ShareDenominator = shareDenominator
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.Equal(AdvertFunctionEnum.Shares, created.Advert.AdvertFunction);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(null)]
    public async Task CreateAdvertShare_Fail_InvalidNumerator(int? shareNumerator)
    {
        // Arrange
        var advert = BaseShareAdvert(AdvertTypeEnum.Flat) with
        {
            ShareNumerator = shareNumerator
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(null)]
    public async Task CreateAdvertShare_Fail_InvalidDenominator(int? invalidDenominator)
    {
        // Arrange
        var advert = BaseShareAdvert(AdvertTypeEnum.Flat) with
        {
            ShareDenominator = invalidDenominator
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateAdvertShare_Fail_MissingNumeratorAndDenominator()
    {
        // Arrange
        var advert = BaseShareAdvert(AdvertTypeEnum.Flat) with
        {
            ShareDenominator = null,
            ShareNumerator = null
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
