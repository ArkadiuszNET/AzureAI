namespace AzureAIServices.ClientFactories;

using Azure;
using Azure.AI.Language.QuestionAnswering;
using AzureAIServices.Options;

public class QuestionAnsweringClientFactory
{
    public static (QuestionAnsweringClient, QuestionAnsweringProject) Create()
    {
        var project = new QuestionAnsweringProject(AzureSettings.Language.ProjectName, AzureSettings.Language.DeploymentName);

        var credentials = new AzureKeyCredential(AzureSettings.Language.Key);
        var client = new QuestionAnsweringClient(new Uri(AzureSettings.Language.Endpoint), credentials);

        return (client, project);
    }
}