namespace AzureAIServices.FacialRecognition;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
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
            BaseAddress = new Uri("https://customvisionignition-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/22ae03d1-4416-4c49-bdb7-8d424533b22c/classify/iterations/ToadstoolPineapple/url"),
        };
        httpClient.DefaultRequestHeaders.Add("Prediction-Key", "71688f97c1c0429e90b80a6bd8a48162");

        var requestContent = new StringContent(JsonConvert.SerializeObject(new Body(imageUrl)), new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await httpClient.PostAsync(string.Empty, requestContent);
        var stringResponse = await response.Content.ReadAsStringAsync(); //response.Content.ReadFromJsonAsync - under the hood it leverage the System.Text.Json
        return JsonConvert.DeserializeObject<ImagePrediction>(stringResponse);
    }

    private record Body(string Url);
}