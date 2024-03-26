namespace AzureAIServices.Options;

using System.Reflection;
using Microsoft.Extensions.Configuration;

public class AzureSettings
{
    private static readonly IConfiguration _configuration;

    static AzureSettings()
    {
        var projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _configuration = new ConfigurationBuilder()
            .SetBasePath(projectDirectory!)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
            .Build();
    }

    public static ServiceOptions ComputerVision => new (_configuration.GetSection("ComputerVision:key").Value!, _configuration.GetSection("ComputerVision:endpoint").Value!);
    public static ServiceOptions FaceAPI => new (_configuration.GetSection("FaceAPI:key").Value!, _configuration.GetSection("FaceAPI:endpoint").Value!);
    public static ServiceOptions CustomVision => new (_configuration.GetSection("CustomVision:key").Value!, _configuration.GetSection("CustomVision:endpoint").Value!);
    public static ServiceOptions Language => new(_configuration.GetSection("Language:key").Value!, _configuration.GetSection("Language:endpoint").Value!,
                                                ProjectName: _configuration.GetSection("Language:projectName").Value!, DeploymentName: _configuration.GetSection("Language:deploymentName").Value!);
    public static ServiceOptions Speech => new (_configuration.GetSection("Speech:key").Value!, _configuration.GetSection("Speech:endpoint").Value!, "westeurope");
}

public record ServiceOptions(string Key, string Endpoint, string? Region = null, string? ProjectName = null, string? DeploymentName = null);