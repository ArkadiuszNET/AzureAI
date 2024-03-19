namespace AzureAIServices.ComputerVision;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Models = Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

public class ImageDescription
{
    private readonly ComputerVisionClient _client;

    public ImageDescription(ComputerVisionClient client)
    {
        _client = client;
    }

    public Task<Models.ImageDescription> SendWebImage(string imageUrl)
    {
        return _client.DescribeImageAsync(imageUrl);
    }
}