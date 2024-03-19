namespace AzureAIServices.ComputerVision;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

public class ImageThumbnail
{
    private readonly ComputerVisionClient _client;

    public ImageThumbnail(ComputerVisionClient client)
    {
        _client = client;
    }

    public Task<Stream> SendWebImage(string imageUrl)
    {
        return _client.GenerateThumbnailAsync(50, 50, imageUrl, true);
    }
}