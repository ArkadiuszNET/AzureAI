namespace AzureAIServices.Services.CustomVision;

using System.Net.Http.Headers;
using AzureAIServices.Options;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Newtonsoft.Json;

public class CustomVisionService
{
    public CustomVisionService()
    {
    }

    public async Task<ImagePrediction?> SendWebImage(string imageUrl)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(AzureSettings.CustomVision.Endpoint),
        };
        httpClient.DefaultRequestHeaders.Add("Prediction-Key", AzureSettings.CustomVision.Key);

        var requestContent = new StringContent(JsonConvert.SerializeObject(new Body(imageUrl)), new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await httpClient.PostAsync(string.Empty, requestContent);
        var stringResponse = await response.Content.ReadAsStringAsync(); //response.Content.ReadFromJsonAsync - under the hood it leverage the System.Text.Json
        return JsonConvert.DeserializeObject<ImagePrediction>(stringResponse);
    }

    private record Body(string Url);
}