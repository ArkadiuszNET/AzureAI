namespace AzureAIServices.ComputerVision;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

public class ImageTagging(ComputerVisionClient client)
{
    private readonly ComputerVisionClient _client = client;

    public async Task<IList<ImageTag>> SendWebImage(string imageUrl)
    {
        var response = await _client.TagImageAsync(imageUrl);

        return response.Tags;
    }
}