namespace AddressProcessor;

public record AddressResult
{
    public string? StreetName { get; set; }
    public string? BuildingNumber { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? CountrySubDivision { get; set; }
    public string? Country { get; set; }
    public List<string>? AddressLines { get; init; }
    public string? ForAttentionOf { get; set; }
    public string? CompanyName { get; set; }
}