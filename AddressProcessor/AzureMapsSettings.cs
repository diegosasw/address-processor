namespace AddressProcessor;

public sealed record AzureMapsSettings
{
    public string SubscriptionKey { get; init; } = string.Empty;
}