namespace AzureAIServices.Services.ComputerVision;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

public class ImageAnalyze
{
    private readonly ComputerVisionClient _client;

    public ImageAnalyze(ComputerVisionClient client)
    {
        _client = client;
    }

    public Task<ImageAnalysis> SendWebImage(string imageUrl)
    {
        var features = new List<VisualFeatureTypes?>
        {
            VisualFeatureTypes.Description,
            VisualFeatureTypes.Objects,
            VisualFeatureTypes.Adult,
            VisualFeatureTypes.Tags
        };
        return _client.AnalyzeImageAsync(imageUrl, features);
    }
}