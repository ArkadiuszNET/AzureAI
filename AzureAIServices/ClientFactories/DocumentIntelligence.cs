namespace AzureAIServices.ClientFactories;

using Azure;
using Azure.AI.DocumentIntelligence;
using AzureAIServices.Options;

public class DocumentIntelligenceClientFactory
{
    public static DocumentIntelligenceClient Create()
    {
        var endpoint = new Uri(AzureSettings.DocumentIntelligence.Endpoint);
        var credential = new AzureKeyCredential(AzureSettings.DocumentIntelligence.Key);
        
        return new DocumentIntelligenceClient(endpoint, credential);
    }
}