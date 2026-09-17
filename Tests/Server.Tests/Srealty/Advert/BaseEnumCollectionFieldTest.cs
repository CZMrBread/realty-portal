using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert;

public interface IEnumCollectionFieldSpec<TEnum> where TEnum : struct, Enum
{
    static abstract string PropertyName { get; }
    
    static abstract AdvertTypeEnum[] RequiredForTypes { get; }

    static abstract SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<TEnum>? value);

    static abstract ICollection<TEnum>? ReadValue(CreateAdvertResponse created);

    static virtual AdvertTypeEnum SampleType => AdvertTypeEnum.House;
}

public abstract class BaseEnumCollectionFieldTest<TSelf, TEnum>(
    SharedAgentFixture agent,
    ITestOutputHelper output)
    : BaseAdvertTest(agent, output)
    where TSelf : IEnumCollectionFieldSpec<TEnum>
    where TEnum : struct, Enum
{
    public static IEnumerable<object[]> ValidValues() => EnumTestData<TEnum>.ValidData();

    public static IEnumerable<object[]> OutOfRangeValues() => EnumTestData<TEnum>.OutOfRangeData();

    private static TEnum[] AllValidValues => ValidValues().Select(row => (TEnum)row[0]).ToArray();

    private static TEnum OutOfRangeSample => (TEnum)OutOfRangeValues().First()[0];


    [Theory]
    [MemberData(nameof(ValidValues))]
    public async Task CreateAdvert_WithASingleValidValue_Succeeds(TEnum value)
    {
        // Arrange
        var advert = TSelf.WithValue(BaseAdvertFor(TSelf.SampleType), [value]);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        Assert.NotEqual(Guid.Empty, created.Advert.AdvertId);
        Assert.Equal([value], TSelf.ReadValue(created));
    }
    
    [Fact]
    public async Task CreateAdvert_WithEveryValidValue_Succeeds()
    {
        // Arrange
        var all = AllValidValues;
        var advert = TSelf.WithValue(BaseAdvertFor(TSelf.SampleType), all);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

        // Assert
        output.WriteLine(created?.Advert?.ToString());
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(created?.Advert);
        var items = TSelf.ReadValue(created);
        Assert.NotNull(items);
        Assert.Equal(all.Order(), items.Order());
    }
    
    [Theory]
    [MemberData(nameof(OutOfRangeValues))]
    public async Task CreateAdvert_WithAValueOutsideTheEnumRange_Fails(TEnum value)
    {
        // Arrange
        var advert = TSelf.WithValue(BaseAdvertFor(TSelf.SampleType), [value]);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(responseString);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(TSelf.PropertyName, responseString);
    }
    
    [Fact]
    public async Task CreateAdvert_WithAnInvalidValueAmongValidOnes_Fails()
    {
        // Arrange
        var items = AllValidValues.Take(1).Append(OutOfRangeSample).ToArray();
        var advert = TSelf.WithValue(BaseAdvertFor(TSelf.SampleType), items);

        // Act
        var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
        var responseString = await response.Content.ReadAsStringAsync();

        // Assert
        output.WriteLine(responseString);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(TSelf.PropertyName, responseString);
    }

    // Facts with an internal loop, not [Theory]/[MemberData]: RequiredForTypes can legitimately be empty
    // (a field that's never required) or cover all five types (always required), and xUnit's [Theory]
    // throws "No data found" for a MemberData source with zero rows - looping here handles 0..5 uniformly,
    // an empty loop being vacuously correct (nothing requires it, so there's nothing to check fails on null).

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
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest,
                $"{type}: expected BadRequest, got {response.StatusCode}. {responseString}");
            Assert.Contains(TSelf.PropertyName, responseString);
        }
    }

    [Fact]
    public async Task CreateAdvert_WithEmptyValue_FailsForTypesThatRequireIt()
    {
        foreach (var type in TSelf.RequiredForTypes)
        {
            // Arrange
            var advert = TSelf.WithValue(BaseAdvertFor(type), []);

            // Act
            var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            output.WriteLine($"{type}: {responseString}");
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest,
                $"{type}: expected BadRequest, got {response.StatusCode}. {responseString}");
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
            Assert.Empty(TSelf.ReadValue(created) ?? []);
        }
    }

    [Fact]
    public async Task CreateAdvert_WithEmptyValue_SucceedsForTypesThatDoNotRequireIt()
    {
        foreach (var type in Enum.GetValues<AdvertTypeEnum>().Except(TSelf.RequiredForTypes))
        {
            // Arrange
            var advert = TSelf.WithValue(BaseAdvertFor(type), []);

            // Act
            var response = await agent.Client.PostAsJsonAsync(ApiBaseUrl, advert);
            var created = await response.Content.ReadFromJsonAsync<CreateAdvertResponse>();

            // Assert
            output.WriteLine($"{type}: {created?.Advert}");
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"{type}: expected OK, got {response.StatusCode}");
            Assert.NotNull(created?.Advert);
            Assert.Empty(TSelf.ReadValue(created) ?? []);
        }
    }
}
