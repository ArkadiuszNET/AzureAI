namespace AzureAIServices.ClientFactories;

using Azure;
using Azure.AI.Vision.ImageAnalysis;
using AzureAIServices.Options;

public static class ImageAnalysisClientFactory
{
    public static ImageAnalysisClient Create()
    {
        var credentials = new AzureKeyCredential(AzureSettings.ComputerVision.Key);
        return new ImageAnalysisClient(new Uri(AzureSettings.ComputerVision.Endpoint), credentials);
    }
}