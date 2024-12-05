using System.Globalization;
using Azure.Maps.Search;
using Azure.Maps.Search.Models;

namespace AddressProcessor;

public class AddressQueryService
{
    private readonly MapsSearchClient _mapsSearchClient;

    public AddressQueryService(MapsSearchClient mapsSearchClient)
        => _mapsSearchClient = mapsSearchClient;

    public async Task<AddressResult> GetAddress(string address)
    {
        var searchResult = await _mapsSearchClient.GetGeocodingAsync(address);

        var bestMatchFeature =
            searchResult.Value.Features
                .OrderByDescending(feature =>
                    GetConfidenceLevel(feature.Properties.Confidence?.ToString() ?? string.Empty))
                .FirstOrDefault();

        if (bestMatchFeature is not null)
        {
            var result = GetAddressResult(bestMatchFeature.Properties.Address);
            return result;
        }
        
        throw new KeyNotFoundException($"Address not found: {address}");
    }

    private string GetCountryCode(string countryName)
    {
        // Normalize the country name for matching with available cultures
        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            var region = new RegionInfo(culture.Name);

            if (region.EnglishName.Equals(countryName, StringComparison.OrdinalIgnoreCase))
            {
                return region.TwoLetterISORegionName;
            }
        }

        throw new ArgumentException($"Country name '{countryName}' not found.");
    }
    
    private static int GetConfidenceLevel(string confidence)
    {
        return confidence switch
        {
            "High" => 3,
            "Medium" => 2,
            "Low" => 1,
            _ => 0
        };
    }

    private AddressResult GetAddressResult(Address address)
    {
        var formattedAddressLines = address.FormattedAddress.Split(',');
        return new AddressResult
        {
            City = address.Locality,
            Country = GetCountryCode(address.CountryRegion.Name),
            PostalCode = address.PostalCode,
            StreetName = formattedAddressLines.ElementAtOrDefault(0),
            AddressLines = formattedAddressLines.ToList()
        };
    }
}