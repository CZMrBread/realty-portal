using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Rents;

public class TestRents(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseAdvertTest(agent, output)
{
    [Fact]
    public async Task CreateAdvertRent_Success()
    {
        // Arrange
        var advert = BaseAdvertFor(AdvertTypeEnum.Flat) with
        {
            AdvertFunction = AdvertFunctionEnum.Rent,
            ReadyDate = new DateOnly()
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.Equal(AdvertFunctionEnum.Rent, created.Advert.AdvertFunction);
    } 
    
    [Theory]
    [MemberData(nameof(EnumTestData<LeaseTypeEnum>.OutOfRangeData), MemberType = typeof(EnumTestData<LeaseTypeEnum>))]
    public async Task CreateAdvertRent_Fail_InvalidLeaseType(LeaseTypeEnum leaseType)
    {
        // Arrange
        var advert = BaseAdvertFor(AdvertTypeEnum.Flat) with
        {
            AdvertFunction = AdvertFunctionEnum.Rent,
            LeaseType = leaseType
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateAdvertRent_Fail_MissingReadyDate()
    {
        // Arrange
        var advert = BaseAdvertFor(AdvertTypeEnum.Flat) with
        {
            AdvertFunction = AdvertFunctionEnum.Rent,
            ReadyDate = null
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
