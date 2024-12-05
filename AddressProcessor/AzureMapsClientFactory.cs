using Azure;
using Azure.Maps.Search;

namespace AddressProcessor;

public static class AzureMapsClientFactory
{
    public static MapsSearchClient Create(AzureMapsSettings azureMapsSettings)
    {
        var credential = new AzureKeyCredential(azureMapsSettings.SubscriptionKey);
        var mapSearchClient = new MapsSearchClient(credential);
        return mapSearchClient;
    }
}