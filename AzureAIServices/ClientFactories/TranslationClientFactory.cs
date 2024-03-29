namespace AzureAIServices.ClientFactories;

using Azure;
using Azure.AI.Translation.Text;
using AzureAIServices.Options;

public class TranslationClientFactory
{
    public static TextTranslationClient Create()
    {
        var config = new AzureKeyCredential(AzureSettings.Multi.Key); //doesn't work with language resource - only multi

        return new TextTranslationClient(config, AzureSettings.Multi.Region);
    }
}