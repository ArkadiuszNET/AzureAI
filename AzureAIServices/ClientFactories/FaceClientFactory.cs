namespace AzureAIServices.ClientFactories;

using AzureAIServices.Options;
using Microsoft.Azure.CognitiveServices.Vision.Face;

public static class FaceClientFactory
{
    public static IFaceClient Create()
    {
        var credentials = new ApiKeyServiceClientCredentials(AzureSettings.FaceAPI.Key);
        return new FaceClient(credentials)
        { 
            Endpoint = AzureSettings.FaceAPI.Endpoint 
        };
    }

}