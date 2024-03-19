namespace AzureAIServices.ClientFactories;

using AzureAIServices.Options;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

public static class ComputerVisionClientFactory
{
    public static ComputerVisionClient Create()
    {
        var credentials = new ApiKeyServiceClientCredentials(AzureSettings.ComputerVision.Key);
        return new ComputerVisionClient(credentials)
        { 
            Endpoint = AzureSettings.ComputerVision.Endpoint 
        };
    }
}