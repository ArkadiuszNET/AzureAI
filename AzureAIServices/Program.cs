﻿using Azure.AI.TextAnalytics;
using AzureAIServices.ClientFactories;
using AzureAIServices.Services.ComputerVision;
using AzureAIServices.Services.FacialRecognition;
using AzureAIServices.Options;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using AzureAIServices.Services;
using AzureAIServices.Services.CustomVision;

Console.WriteLine("Azure Cognitive Services - .NET quickstart example");
Console.WriteLine();

var faceClient = FaceClientFactory.Create();
var languageClient = LanguageClientFactory.Create();
var speechRecognizerClient = SpeechClientFactory.CreateRecognizer();
var (translateSyntethizerClient, translateClient) = SpeechClientFactory.CreateTranslator();

var serviceToRun = ServiceType.SpeechToText;

switch(serviceToRun)
{
    case ServiceType.OCR:
        await ExecuteOCR();
        break;
    case ServiceType.ImageTagging:
        await ExecuteImageTagging();
        break;
    case ServiceType.ImageDescribing:
        await ExecuteImageDescribing();
        break;
    case ServiceType.ImageAnalyze:
        await ExecuteImageAnalyze();
        break;
    case ServiceType.ImageThumbnail:
        await ExecuteImageThumbnail();
        break;
    case ServiceType.FacialAttributes:
        await ExecuteFacialAttributes(); //won't work till form of usage is filled
        break;
    case ServiceType.CustomVision:
        await ExecuteCustomVision();
        break;
    case ServiceType.ExtractEntityInformation:
        await ExecuteExtractEntityInformation();
        break;
    case ServiceType.SentimentAnalysis:
        await ExecuteSentimentAnalysis();
        break;
    case ServiceType.LanguageDetection:
        await ExecuteLanguageDetection();
        break;
    case ServiceType.TextToSpeech:
        await ExecuteTextToSpeech();
        break;
    case ServiceType.SpeechToText:
        await ExecuteSpeechToText();
        break;
    case ServiceType.SpeechToSpeechTranslation:
        break;
    default:
        Console.WriteLine($"Unhandled service type: {serviceToRun}");
        break;
}




#region Speech to speech translation

// Console.WriteLine("Speak...");

// var result = await translateClient.RecognizeOnceAsync();

// Console.WriteLine("Processing...");

// await OutputSpeechSynthesisResult(result);

// async Task OutputSpeechSynthesisResult(TranslationRecognitionResult result)
// {
//     switch (result.Reason)
//     {
//         case ResultReason.TranslatedSpeech:
//             Console.WriteLine($"Speech recognized: {result.Text}");

//             foreach(var key in result.Translations){
//                 Console.WriteLine($"Translated into {key.Key} - {key.Value}");
//                 await translateSyntethizerClient.SpeakTextAsync(key.Value);
//             }

//             break;
//         case ResultReason.Canceled:
//             Console.WriteLine($"CANCELED: Reason={result.Reason}");
//             break;
//         case ResultReason.NoMatch:
//             Console.WriteLine($"Not match: Reason={result.Reason}");
//             break;
//         default:
//             Console.WriteLine($"Unknown result: Reason={result.Reason}");
//             break;
//     }
// }

#endregion

Console.WriteLine("----------------------------------------------------------");
Console.WriteLine();
Console.WriteLine("Quickstart is complete.");
Console.WriteLine();


async Task ExecuteOCR()
{
    var imageUrlToAnalyze = "https://d1fa9n6k2ql7on.cloudfront.net/H0O8LX1S0W4MYIT1661555421.png"; //simple invoice file

    var computerVisionClient = ComputerVisionClientFactory.Create();
    var analyzeImageService = new ImageOCR(computerVisionClient);
    var acceptedOperationId = await analyzeImageService.SendWebImage(imageUrlToAnalyze);
    var result = await analyzeImageService.GetImageAnalysis(acceptedOperationId);

    Console.WriteLine("Image OCR analysis result");
    Console.WriteLine();

    var lines = result.AnalyzeResult.ReadResults.SelectMany(x => x.Lines);

    foreach(var line in lines){
        Console.WriteLine(line.Text);
    }
}

async Task ExecuteImageTagging()
{
    // women photo
    var imageUrlToAnalyze = "https://images.unsplash.com/photo-1599140781162-68659a79e313?q=80&w=2976&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";

    var computerVisionClient = ComputerVisionClientFactory.Create();
    var imageTaggingService = new ImageTagging(computerVisionClient);
    var imageTags = await imageTaggingService.SendWebImage(imageUrlToAnalyze);
    Console.WriteLine("Image tags: ");

    foreach(var tag in imageTags){
        Console.WriteLine($"{tag.Name} - {tag.Confidence} [{tag.Hint}]");
    }
}

async Task ExecuteImageDescribing()
{
    // bearded men photo
    var imageUrlToAnalyze = "https://images.unsplash.com/photo-1480455624313-e29b44bbfde1?q=80&w=3270&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";

    var computerVisionClient = ComputerVisionClientFactory.Create();
    var service = new ImageDescription(computerVisionClient);
    var response = await service.SendWebImage(imageUrlToAnalyze);
    Console.WriteLine("Image description: ");

    foreach(var capition in response.Captions){
        Console.WriteLine($"{capition.Text} - {capition.Confidence}");
    }

    Console.WriteLine($"Tags: {string.Join(',',response.Tags)}");
}

async Task ExecuteImageAnalyze()
{
    // small wound
    var imageUrlToAnalyze = "https://www.ehbo.nl/media/1016/wond.png";

    var computerVisionClient = ComputerVisionClientFactory.Create();
    var service = new ImageAnalyze(computerVisionClient);
    var response = await service.SendWebImage(imageUrlToAnalyze);

    foreach(var @object in response.Objects){
        Console.WriteLine($"Object: {@object.ObjectProperty}");
    }

    foreach(var tag in response.Tags){
        Console.WriteLine($"Tag: {tag.Name} - {tag.Confidence}");
    }

    Console.WriteLine($"Is Racy: {response.Adult.IsRacyContent} - [{response.Adult.RacyScore}]");
    Console.WriteLine($"Is Gore: {response.Adult.IsGoryContent} - [{response.Adult.GoreScore}]");
    Console.WriteLine($"Is Adult: {response.Adult.IsAdultContent} - [{response.Adult.AdultScore}]");
}

async Task ExecuteImageThumbnail()
{
    var imageUrl = "https://plus.unsplash.com/premium_photo-1686754185788-12b50c02521a?q=80&w=3270&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";
    var outputFilePath = "/Users/arkadiuszoleksy/Desktop/thumb.png";

    var computerVisionClient = ComputerVisionClientFactory.Create();
    var service = new ImageThumbnail(computerVisionClient);
    var response = await service.SendWebImage(imageUrl);

    var createdFile = File.Create(outputFilePath);
    await response.CopyToAsync(createdFile);
    createdFile.Close();
}

async Task ExecuteFacialAttributes()
{
    // woman with mask
    var imageUrl = "https://scitechdaily.com/images/Woman-Putting-on-COVID-Face-Mask.jpg";
    var facialAttributesService = new FacialAttributes(faceClient);

    var result = await facialAttributesService.SendWebImage(imageUrl);

    Console.WriteLine($"Detected faces count: {result.Count}");

    foreach(var face in result)
    {
        Console.WriteLine("Face attributes:");
        Console.WriteLine($"Makeup: Eye: {face.FaceAttributes.Makeup.EyeMakeup} Lip: {face.FaceAttributes.Makeup.LipMakeup}");
        Console.WriteLine($"Age: {face.FaceAttributes.Age}");
        Console.WriteLine($"Accessories: {face.FaceAttributes.Accessories}");
        foreach(var accessory in face.FaceAttributes.Accessories){
            Console.WriteLine($"{accessory.Type} - {accessory.Confidence}");
        }
        Console.WriteLine($"Gender: {face.FaceAttributes.Gender.GetValueOrDefault()}");
    }
}

async Task ExecuteCustomVision()
{
    // pineapple
    var imageUrl = "https://www.fruit4london.co.uk/cdn/shop/products/Pineapple.jpg?v=1683266629";

    var service = new CustomVisionService();
    var result = await service.SendWebImage(imageUrl);

    if(result != null)
    {
        Console.WriteLine(result.Predictions);

        foreach(var prediction in result.Predictions)
        {
            Console.WriteLine($"Prediction: {prediction.TagName} - {prediction.Probability}");
        }
    }
    else
    {
        Console.WriteLine("Custom vision - empty result");
    }
}

async Task ExecuteExtractEntityInformation() 
{
    var textToDetect = "Microsoft Azure, often referred to as Azure is a cloud computing platform run by Microsoft. It offers access, management, and the development of applications and services through global data centers. It also provides a range of capabilities, including software as a service (SaaS), platform as a service (PaaS), and infrastructure as a service (IaaS). Microsoft Azure supports many programming languages, tools, and frameworks, including Microsoft-specific and third-party software and systems. Azure was first introduced at the Professional Developers Conference (PDC) in October 2008 under the codename 'Project Red Dog.' It was officially launched as Windows Azure in February 2010 and later renamed Microsoft Azure on March 25, 2014";

    var response = await languageClient.RecognizeEntitiesAsync(textToDetect);

    Console.WriteLine($"Text to detect: \n\n{textToDetect}\n\n");

    foreach(var phrase in response.Value){
        Console.WriteLine($"{phrase.Text} - {phrase.Category}[{phrase.SubCategory}] - {phrase.ConfidenceScore}");
    }
}

async Task ExecuteSentimentAnalysis()
{
    var sentimentInput = "The recuritment process consisting of interview with HR department is a waste of time - but might be beneficial for company maybe";
    var options = new AnalyzeSentimentOptions
    {
        IncludeStatistics = true,
    };

    var response = await languageClient.AnalyzeSentimentAsync(sentimentInput, "en", options);

    Console.WriteLine($"The opinion: {sentimentInput}");
    Console.WriteLine($"Sentiment: {response.Value.Sentiment}");

    Console.WriteLine($"Positive score: {response.Value.ConfidenceScores.Positive}");
    Console.WriteLine($"Neutral score:  {response.Value.ConfidenceScores.Neutral}");
    Console.WriteLine($"Negative score:  {response.Value.ConfidenceScores.Negative}");
}

async Task ExecuteLanguageDetection()
{
    var languageDetectInput = "Mucho gusto, mi nombre es";

    var response = await languageClient.DetectLanguageAsync(languageDetectInput, null, CancellationToken.None);

    Console.WriteLine($"The language: {response.Value.Name}[{response.Value.Iso6391Name}] - {response.Value.ConfidenceScore}");
}

async Task ExecuteTextToSpeech()
{
    var textToSpeechInput = "Nie czas na kawę!";
    var filePathWithSynthesizedText = "/Users/arkadiuszoleksy/Desktop/text.wav";

    var speechClient = SpeechClientFactory.Create();
    var result = await speechClient.SpeakTextAsync(textToSpeechInput);
    OutputSpeechSynthesisResult(result, textToSpeechInput);

    void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
    {
        switch (speechSynthesisResult.Reason)
        {
            case ResultReason.SynthesizingAudioCompleted:
                Console.WriteLine($"Speech synthesized for text: [{text}]");
                File.WriteAllBytes(filePathWithSynthesizedText, speechSynthesisResult!.AudioData);
                break;
            case ResultReason.Canceled:
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
            default:
                break;
        }
    }
}

async Task ExecuteSpeechToText()
{
    Console.WriteLine("Speak to microphone...");

    var result = await speechRecognizerClient.RecognizeOnceAsync();

    Console.WriteLine("Processing...");

    OutputSpeechSynthesisResult(result);

    static void OutputSpeechSynthesisResult(SpeechRecognitionResult speechSynthesisResult)
    {
        switch (speechSynthesisResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Console.WriteLine($"Speech recognized: {speechSynthesisResult.Text}");
                break;
            case ResultReason.Canceled:
                Console.WriteLine($"CANCELED: Reason={speechSynthesisResult.Reason}");
                break;
            case ResultReason.NoMatch:
                Console.WriteLine($"Not match: Reason={speechSynthesisResult.Reason}");
                break;
            default:
                break;
        }
    }
}