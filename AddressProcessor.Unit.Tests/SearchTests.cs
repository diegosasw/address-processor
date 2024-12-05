using AddressProcessor.Unit.Tests.Extensions;
using Azure.Maps.Search;
using FluentAssertions;

namespace AddressProcessor.Unit.Tests;

public class SearchTests
{
    private readonly MapsSearchClient _mapSearchClient;

    public SearchTests()
    {
        var settings =
            new AzureMapsSettings()
            {
                SubscriptionKey = "TO_REPLACE"
            };
        _mapSearchClient = AzureMapsClientFactory.Create(settings);
    }
    
    [Theory]
    [MemberData(nameof(MemberData.AddressAndExpectedResult), MemberType = typeof(MemberData))]
    public async Task Given_Address_String_It_Should_Return_Expected_Result(string address, AddressResult expectedResult)
    {
        var sut = new AddressQueryService(_mapSearchClient);
        var result = await sut.GetAddress(address);
        result.City.Should().BeEquivalentTo(expectedResult.City);
        result.Country.Should().BeEquivalentTo(expectedResult.Country);
        result.PostalCode.Should().BeEquivalentTo(expectedResult.PostalCode);
        result.AddressLines.Should().NotBeEmpty();
    }
}

static class MemberData
{
    public static IEnumerable<object[]> AddressAndExpectedResult
        => new List<(string, AddressResult)>
        {
            (
                "Avenida San Javier 67, 29140 Malaga",
                new AddressResult
                {
                    City = "Málaga",
                    Country = "ES",
                    PostalCode = "29140",
                }
            ),
            (
                "47 Chester Rd, London N19 5DF, United Kingdom",
                new AddressResult
                {
                    City = "London",
                    Country = "GB",
                    PostalCode = "N19 5DF",
                }
            )
        }.ToMemberData();
}