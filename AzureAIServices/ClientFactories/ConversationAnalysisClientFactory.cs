namespace AzureAIServices.ClientFactories;

using Azure;
using Azure.AI.Language.Conversations;
using AzureAIServices.Options;

public class ConversationAnalysisClientFactory
{
    public static ConversationAnalysisClient Create()
    {
        var uri = new Uri(AzureSettings.Language.Endpoint);
        var credentials = new AzureKeyCredential(AzureSettings.Language.Key);

        return new ConversationAnalysisClient(uri, credentials);
    }    
}