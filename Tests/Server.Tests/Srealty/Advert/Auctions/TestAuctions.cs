using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Auctions;

public class TestAuctions(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseAdvertTest(agent, output)
{
    public SrealityAdvertDto BaseAuctionAdvert(AdvertTypeEnum advertType) =>
        BaseAdvertFor(advertType) with
        {
            AdvertFunction = AdvertFunctionEnum.Auction,
            AuctionKind = AuctionKindEnum.Involuntary,
            AuctionDate = new DateTimeOffset(),
            Personal = 1,
            AuctionPlace = "Prague",
            Bidding = BiddingTypeEnum.English,
            PriceExpertReport = 100000,
            PriceMinimumBid = 50000,
        };
    [Fact]
    public async Task CreateAdvertAuction_Success()
    {
        // Arrange
        var advert = BaseAuctionAdvert(AdvertTypeEnum.Flat);
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.Equal(AdvertFunctionEnum.Auction, created.Advert.AdvertFunction);
    }
    
    [Theory]
    [MemberData(nameof(EnumTestData<AuctionKindEnum>.OutOfRangeData), MemberType = typeof(EnumTestData<AuctionKindEnum>))]
    public async Task CreateAdvertAuction_Fail_InvalidAuctionKind(AuctionKindEnum auctionKind)
    {
        // Arrange
        var advert = BaseAuctionAdvert(AdvertTypeEnum.Flat) with
        {
            AuctionKind = auctionKind
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [MemberData(nameof(EnumTestData<BiddingTypeEnum>.OutOfRangeData), MemberType = typeof(EnumTestData<BiddingTypeEnum>))]
    public async Task CreateAdvertAuction_Fail_InvalidBiddingType(BiddingTypeEnum biddingType)
    {
        // Arrange
        var advert = BaseAuctionAdvert(AdvertTypeEnum.Flat) with
        {
            Bidding = biddingType
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateAdvertAuction_Fail_MissingBiddingTypeForEnglishAuction()
    {
        // Arrange
        var advert = BaseAuctionAdvert(AdvertTypeEnum.Flat) with
        {
            Bidding = BiddingTypeEnum.English,
            PriceMinimumBid = null
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    } 
    
    [Theory]
    [InlineData(AuctionKindEnum.Involuntary)]
    [InlineData(AuctionKindEnum.Enforcement)]
    public async Task CreateAdvertAuction_Fail_MissingPriceExpertReportForAuctions(AuctionKindEnum auctionKind)
    {
        // Arrange
        var advert = BaseAuctionAdvert(AdvertTypeEnum.Flat) with
        {
            AuctionKind = auctionKind,
            PriceExpertReport = null
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(created);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Theory]
    [MemberData(nameof(EnumTestData<AuctionKindEnum>.ValidData), MemberType = typeof(EnumTestData<AuctionKindEnum>))]
    public async Task CreateAdvertAuction_Success_AuctionKindCheck(AuctionKindEnum auctionKind)
    {
        // Arrange
        var advert = BaseAuctionAdvert(AdvertTypeEnum.Flat) with
        {
            AuctionKind = auctionKind
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.Equal(AdvertFunctionEnum.Auction, created.Advert.AdvertFunction);
    }
    
    [Theory]
    [MemberData(nameof(EnumTestData<BiddingTypeEnum>.ValidData), MemberType = typeof(EnumTestData<BiddingTypeEnum>))]
    public async Task CreateAdvertAuction_Success_BiddingTypesCheck(BiddingTypeEnum biddingType)
    {
        // Arrange
        var advert = BaseAuctionAdvert(AdvertTypeEnum.Flat) with
        {
            Bidding = biddingType
        };
        
        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.Equal(AdvertFunctionEnum.Auction, created.Advert.AdvertFunction);
    }
}
