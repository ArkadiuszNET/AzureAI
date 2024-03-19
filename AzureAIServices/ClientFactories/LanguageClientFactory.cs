namespace AzureAIServices.ClientFactories;

using Azure;
using Azure.AI.TextAnalytics;
using AzureAIServices.Options;

public static class LanguageClientFactory
{
    public static TextAnalyticsClient Create()
    {
        var credentials = new AzureKeyCredential(AzureSettings.Language.Key);
        var serviceEndpoint = new Uri(AzureSettings.Language.Endpoint);

        return new TextAnalyticsClient(serviceEndpoint, credentials);
    }
}