namespace AzureAIServices.ClientFactories;

using AzureAIServices.Options;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;

public static class CustomVisionClientFactory
{
    public static CustomVisionPredictionClient Create()
    {
        var credentials = new ApiKeyServiceClientCredentials(AzureSettings.CustomVision.Key);
        return new CustomVisionPredictionClient(credentials)
        { 
            Endpoint = AzureSettings.CustomVision.Endpoint,
        };
    }
}