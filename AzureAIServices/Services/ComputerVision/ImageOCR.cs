namespace AzureAIServices.Services.ComputerVision;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

public class ImageOCR
{
    private readonly ComputerVisionClient _client;
    private const int NumberOfCharsInOperationId = 36;

    public ImageOCR(ComputerVisionClient client)
    {
        _client = client;
    }

    public async Task<Guid> SendWebImage(string imageUrl)
    {
        var response = await _client.ReadAsync(imageUrl);

        return GetOperationId(response.OperationLocation);
    }

    public async Task<Guid> SendLocalImage(string imageUrl)
    {
        var response = await _client.ReadInStreamAsync(File.OpenRead(imageUrl));
        
        return GetOperationId(response.OperationLocation);
    }

    public async Task<ReadOperationResult> GetImageAnalysis(Guid acceptedOperationId)
    {
        ReadOperationResult analysisResult;

        do
        {
            analysisResult = await _client.GetReadResultAsync(acceptedOperationId);
            await Task.Delay(1000);       
            Console.WriteLine("Processing..."); 
        }
        while(NotFinished());

        return analysisResult;

        bool NotFinished() => analysisResult.Status == OperationStatusCodes.Running || analysisResult.Status == OperationStatusCodes.NotStarted;
    }

    private static Guid GetOperationId(string operationLocation)
    {
        var operationId = operationLocation.Substring(operationLocation.Length - NumberOfCharsInOperationId); 
        return new Guid(operationId);
    }
}
