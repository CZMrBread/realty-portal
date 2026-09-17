using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert;

public interface IEnumFieldSpec<TEnum> where TEnum : struct, Enum
{
    static abstract string PropertyName { get; }
    
    static abstract AdvertTypeEnum[] RequiredForTypes { get; }

    static abstract SrealityAdvertDto WithValue(SrealityAdvertDto advert, TEnum? value);

    static abstract TEnum? ReadValue(CreateAdvertResponse created);

    static virtual AdvertTypeEnum SampleType => AdvertTypeEnum.House;
}

public abstract class BaseEnumFieldTest<TSelf, TEnum>(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseAdvertTest(agent, output)
    where TSelf : IEnumFieldSpec<TEnum>
    where TEnum : struct, Enum
{
    public static IEnumerable<object[]> ValidValues() => EnumTestData<TEnum>.ValidData();

    public static IEnumerable<object[]> OutOfRangeValues() => EnumTestData<TEnum>.OutOfRangeData();


    [Theory]
    [MemberData(nameof(ValidValues))]
    public async Task CreateAdvert_WithAValidValue_Succeeds(TEnum value)
    {
        // Arrange
        var advert = TSelf.WithValue(BaseAdvertFor(TSelf.SampleType), value);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.NotEqual(Guid.Empty, created.Advert.AdvertId);
        Assert.Equal(value, TSelf.ReadValue(created));
    }
    
    [Theory]
    [MemberData(nameof(OutOfRangeValues))]
    public async Task CreateAdvert_WithAValueOutsideTheEnumRange_Fails(TEnum value)
    {
        // Arrange
        var advert = TSelf.WithValue(BaseAdvertFor(TSelf.SampleType), value);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(responseString);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(TSelf.PropertyName, responseString);
    }
    
    [Fact]
    public async Task CreateAdvert_WithNullValue_FailsForTypesThatRequireIt()
    {
        foreach (var type in TSelf.RequiredForTypes)
        {
            // Arrange
            var advert = TSelf.WithValue(BaseAdvertFor(type), null);

            // Act
            var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            output.WriteLine($"{type}: {responseString}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(TSelf.PropertyName, responseString);
        }
    }

    [Fact]
    public async Task CreateAdvert_WithNullValue_SucceedsForTypesThatDoNotRequireIt()
    {
        foreach (var type in Enum.GetValues<AdvertTypeEnum>().Except(TSelf.RequiredForTypes))
        {
            // Arrange
            var advert = TSelf.WithValue(BaseAdvertFor(type), null);

            // Act
            var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
            var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

            // Assert
            output.WriteLine($"{type}: {created?.Advert}");
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"{type}: expected OK, got {response.StatusCode}");
            Assert.NotNull(created?.Advert);
            Assert.NotEqual(Guid.Empty, created.Advert.AdvertId);
            Assert.Null(TSelf.ReadValue(created));
        }
    }
}
