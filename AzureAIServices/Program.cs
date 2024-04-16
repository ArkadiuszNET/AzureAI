using Azure.AI.TextAnalytics;
using AzureAIServices.ClientFactories;
using AzureAIServices.Services.ComputerVision;
using AzureAIServices.Services.FacialRecognition;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using AzureAIServices.Services;
using AzureAIServices.Services.CustomVision;
using Azure.AI.Vision.ImageAnalysis;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using AzureAIServices.Options;
using Azure.AI.Language.QuestionAnswering;
using Azure.Core;
using Azure;
using Azure.Core.Serialization;
using AzureAIServices.Utils;
using Azure.AI.Translation.Text;

var serviceToRun = ServiceType.RemoveImageBackground;

Console.WriteLine("Azure Cognitive Services - .NET quickstart example");
Console.WriteLine();

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
        await ExecuteImageAnalyze(); // more general image analyse - indicates whether content is gore or adult. Returns tags, faces etc
        break;
    case ServiceType.ImageAnalyzeExtended:
        await ExecuteImageAnalyzeExtended(); //handles various analysing tasks related to image - landmarks, objects, OCR, people, etc. Encompas granular functionality under one endpoint
        break;
    case ServiceType.RemoveImageBackground:
        await ExecuteRemoveImageBackground();
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
    case ServiceType.ExtractKeyPhrases:
        await ExecuteExtractKeyPhrases();
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
    case ServiceType.TextToSpeechSSML:
        await ExecuteTextToSpeechSSML();
        break;
    case ServiceType.SpeechToText:
        await ExecuteSpeechToText();
        break;
    case ServiceType.SpeechToSpeechTranslation:
        await ExecuteSpeechToSpeechTranslation();
        break;
    case ServiceType.QuestionAnswering:
        await ExecuteQuestionAnswering();
        break;
    case ServiceType.ConversationAnalysis:
        await ExecuteConversationAnalysis();
        break;
    case ServiceType.ClassifyText:
        await ExecuteClassifyText();
        break;
    case ServiceType.CustomNamedEntityRecognition:
        await ExecuteCustomNamedEntityRecognition();
        break;
    case ServiceType.Translator:
        await ExecuteTranslator();
        break;
    default:
        Console.WriteLine($"Unhandled service type: {serviceToRun}");
        break;
}

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
    var imageUrlToAnalyze = "https://plus.unsplash.com/premium_photo-1670006474596-bed5f414228c?q=80&w=3135&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";

    var computerVisionClient = ComputerVisionClientFactory.Create();
    var service = new ImageAnalyze(computerVisionClient);
    var response = await service.SendWebImage(imageUrlToAnalyze);

    foreach(var @object in response.Objects){
        Console.WriteLine($"Object: {@object.ObjectProperty}");
    }

    foreach(var @object in response.Faces){
        Console.WriteLine($"Face: {@object.Gender} - {@object.Age} - [{@object.FaceRectangle.Left}-{@object.FaceRectangle.Top} ({@object.FaceRectangle.Width})]");
    }

    foreach(var tag in response.Tags){
        Console.WriteLine($"Tag: {tag.Name} - {tag.Confidence}");
    }

    Console.WriteLine($"Is Racy: {response.Adult.IsRacyContent} - [{response.Adult.RacyScore}]");
    Console.WriteLine($"Is Gore: {response.Adult.IsGoryContent} - [{response.Adult.GoreScore}]");
    Console.WriteLine($"Is Adult: {response.Adult.IsAdultContent} - [{response.Adult.AdultScore}]");
}

async Task ExecuteImageAnalyzeExtended()
{
    var client = ImageAnalysisClientFactory.Create();

    var imageUrl = new Uri("https://images.unsplash.com/photo-1579451619868-0ae6573504e8?q=80&w=3087&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D");
    var visualFeatures = VisualFeatures.Caption | 
        VisualFeatures.DenseCaptions | 
        VisualFeatures.Objects |
        VisualFeatures.Tags |
        VisualFeatures.People | 
        VisualFeatures.Read;
    
    var binaryData = new BinaryData(File.ReadAllBytes("images/IMG_6848.png")); //handwritten

    var result = await client.AnalyzeAsync(imageUrl, visualFeatures, default, CancellationToken.None);

    if(result.Value.Read != null)
    {
        Console.WriteLine("Obtained text lines: ");
        foreach(var line in result.Value.Read.Blocks.SelectMany(x => x.Lines)){
            Console.WriteLine(line.Text);
            foreach(var word in line.Words){
                Console.WriteLine($"\t{word.Text} - {word.Confidence}");
            }
        }
    }

    // Display analysis results
    // Get image captions
    if (result.Value.Caption.Text != null)
    {
        Console.WriteLine(" Caption:");
        Console.WriteLine($"   \"{result.Value.Caption.Text}\", Confidence {result.Value.Caption.Confidence:0.00}\n");
    }

    // Get image dense captions
    Console.WriteLine(" Dense Captions:");
    foreach (DenseCaption denseCaption in result.Value.DenseCaptions.Values)
    {
        Console.WriteLine($"   Caption: '{denseCaption.Text}', Confidence: {denseCaption.Confidence:0.00}");
    }

    // Get image tags
    if (result.Value.Tags.Values.Count > 0)
    {
        Console.WriteLine($"\n Tags:");
        foreach (DetectedTag tag in result.Value.Tags.Values)
        {
            Console.WriteLine($"   '{tag.Name}', Confidence: {tag.Confidence:F2}");
        }
    }

    // Get objects in the image
    if (result.Value.Objects.Values.Count > 0)
    {
        Console.WriteLine(" Objects:");

        // USE IMAGE SHARP INSTEAD - System.Drawing is designed for windows, not cross platform
        // // Prepare image for drawing
        // Image image = Image.FromFile("/Users/arkadiuszoleksy/Documents/Projects/Azure/AzureAI/AzureAIServices/images/man_street.jpg");
        // Graphics graphics = Graphics.FromImage(image);
        // Pen pen = new Pen(Color.Cyan, 3);
        // Font font = new Font("Arial", 16);
        // SolidBrush brush = new SolidBrush(Color.WhiteSmoke);

        foreach (DetectedObject detectedObject in result.Value.Objects.Values)
        {
            Console.WriteLine($"   \"{detectedObject.Tags[0].Name}\"");

            // // Draw object bounding box
            // var r = detectedObject.BoundingBox;
            // Rectangle rect = new Rectangle(r.X, r.Y, r.Width, r.Height);
            // graphics.DrawRectangle(pen, rect);
            // graphics.DrawString(detectedObject.Tags[0].Name,font,brush,(float)r.X, (float)r.Y);
        }

        // // Save annotated image
        // String output_file = "objects.jpg";
        // image.Save(output_file);
        // Console.WriteLine("  Results saved in " + output_file + "\n");
    }


    // Get people in the image
    if (result.Value.People.Values.Count > 0)
    {
        Console.WriteLine($" People:");

        // Prepare image for drawing
        // System.Drawing.Image image = System.Drawing.Image.FromFile(imageFile);
        // Graphics graphics = Graphics.FromImage(image);
        // Pen pen = new Pen(Color.Cyan, 3);
        // Font font = new Font("Arial", 16);
        // SolidBrush brush = new SolidBrush(Color.WhiteSmoke);

        foreach (DetectedPerson person in result.Value.People.Values)
        {
            // Draw object bounding box
            // var r = person.BoundingBox;
            // Rectangle rect = new Rectangle(r.X, r.Y, r.Width, r.Height);
            // graphics.DrawRectangle(pen, rect);

            // Return the confidence of the person detected
            Console.WriteLine($"   Bounding box {person.BoundingBox.ToString()}, Confidence: {person.Confidence:F2}");
        }

        // Save annotated image
        // String output_file = "persons.jpg";
        // image.Save(output_file);
        // Console.WriteLine("  Results saved in " + output_file + "\n");
    }
}

async Task ExecuteRemoveImageBackground()
{
    // Remove the background from the image or generate a foreground matte
    Console.WriteLine($" Background removal:");
    // Define the API version and mode
    string apiVersion = "2023-02-01-preview";
    string mode = "backgroundRemoval"; // Can be "foregroundMatting" or "backgroundRemoval"

    string url = $"computervision/imageanalysis:segment?api-version={apiVersion}&mode={mode}";

    var filePathWithResult = "/Users/arkadiuszoleksy/Desktop/arek_final.png";

    // Make the REST call
    using (var client = new HttpClient())
    {
        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
        client.BaseAddress = new Uri(AzureSettings.ComputerVision.Endpoint);
        client.DefaultRequestHeaders.Accept.Add(contentType);
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AzureSettings.ComputerVision.Key);

        // You can change the url to use other images in the images folder,
        // such as "building.jpg" or "person.jpg" to see different results.
        var data = new
        {
            url="https://raw.githubusercontent.com/ArkadiuszNET/AzureAI/master/arek.png"
        };

        var jsonData = JsonSerializer.Serialize(data);
        var contentData = new StringContent(jsonData, Encoding.UTF8, contentType);
        var response = await client.PostAsync(url, contentData);

        if (response.IsSuccessStatusCode) {
            File.WriteAllBytes(filePathWithResult, response.Content.ReadAsByteArrayAsync().Result);
            Console.WriteLine("  Results saved in background.png\n");
        }
        else
        {
            Console.WriteLine($"API error: {response.ReasonPhrase} - Check your body url, key, and endpoint.");
        }
    }
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

    var faceClient = FaceClientFactory.Create();
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

    var languageClient = LanguageClientFactory.Create();
    var response = await languageClient.RecognizeEntitiesAsync(textToDetect);

    Console.WriteLine($"Text to detect: \n\n{textToDetect}\n\n");

    foreach(var phrase in response.Value){
        Console.WriteLine($"{phrase.Text} - {phrase.Category}[{phrase.SubCategory}] - {phrase.ConfidenceScore}");
    }
}

async Task ExecuteExtractKeyPhrases() 
{
    var textToDetect = "Microsoft Azure, often referred to as Azure is a cloud computing platform run by Microsoft. It offers access, management, and the development of applications and services through global data centers. It also provides a range of capabilities, including software as a service (SaaS), platform as a service (PaaS), and infrastructure as a service (IaaS). Microsoft Azure supports many programming languages, tools, and frameworks, including Microsoft-specific and third-party software and systems. Azure was first introduced at the Professional Developers Conference (PDC) in October 2008 under the codename 'Project Red Dog.' It was officially launched as Windows Azure in February 2010 and later renamed Microsoft Azure on March 25, 2014";

    var languageClient = LanguageClientFactory.Create();
    var response = await languageClient.ExtractKeyPhrasesAsync(textToDetect);

    Console.WriteLine($"Text to detect: \n\n{textToDetect}\n\n");

    foreach(var phrase in response.Value){
        Console.WriteLine($"{phrase}");
    }
}

async Task ExecuteSentimentAnalysis()
{
    var sentimentInput = "The recuritment process consisting of interview with HR department is a waste of time - but might be beneficial for company maybe";
    var options = new AnalyzeSentimentOptions
    {
        IncludeStatistics = true,
    };

    var languageClient = LanguageClientFactory.Create();
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

    var languageClient = LanguageClientFactory.Create();
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

async Task ExecuteTextToSpeechSSML()
{
    string responseSsml = $@"
        <speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
            <voice name='en-GB-LibbyNeural'>
                Hello dear students!
                <break strength='weak'/>
                Time to end this lab!
            </voice>
        </speak>";
    var filePathWithSynthesizedText = "/Users/arkadiuszoleksy/Desktop/text.wav";

    var speechClient = SpeechClientFactory.Create();
    var result = await speechClient.SpeakSsmlAsync(responseSsml);
    OutputSpeechSynthesisResult(result, responseSsml);

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
    var speechRecognizerClient = SpeechClientFactory.CreateRecognizer();

    Console.WriteLine("Speak to microphone...");
    var result = await speechRecognizerClient.RecognizeOnceAsync();

    Console.WriteLine("Processing...");

    OutputSpeechSynthesisResult(result);

    static void OutputSpeechSynthesisResult(SpeechRecognitionResult speechSynthesisResult)
    {
        switch (speechSynthesisResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                var languageDetected = AutoDetectSourceLanguageResult.FromResult(speechSynthesisResult);
                Console.WriteLine($"Speech recognized: {speechSynthesisResult.Text} [{languageDetected.Language}]");
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

async Task ExecuteSpeechToSpeechTranslation()
{
    var (translateSyntethizerClient, translateClient) = SpeechClientFactory.CreateTranslator();

    Console.WriteLine("Speak to microphone in polish language...");

    var result = await translateClient.RecognizeOnceAsync();

    Console.WriteLine("Processing...");

    await OutputSpeechSynthesisResult(result);

    async Task OutputSpeechSynthesisResult(TranslationRecognitionResult result)
    {
        switch (result.Reason)
        {
            case ResultReason.TranslatedSpeech:
                Console.WriteLine($"Speech recognized: {result.Text}");

                foreach(var key in result.Translations){
                    Console.WriteLine($"Translated into {key.Key} - {key.Value}");
                    await translateSyntethizerClient.SpeakTextAsync(key.Value);
                }

                break;
            case ResultReason.Canceled:
                Console.WriteLine($"CANCELED: Reason={result.Reason}");
                break;
            case ResultReason.NoMatch:
                Console.WriteLine($"Not match: Reason={result.Reason}");
                break;
            default:
                Console.WriteLine($"Unknown result: Reason={result.Reason}");
                break;
        }
    }
}

async Task ExecuteQuestionAnswering()
{
    var (client, project) = QuestionAnsweringClientFactory.Create();

    Console.Write("Question: ");
    var question = Console.ReadLine() ?? string.Empty;

    while(question.ToLower() != "quit")
    {
        var response = await client.GetAnswersAsync(question, project);
        foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
        {
            Console.WriteLine(answer.Answer);
            Console.WriteLine($"Confidence: {answer.Confidence:P2}");
            Console.WriteLine($"Source: {answer.Source}");
            Console.WriteLine();
        }

        Console.Write("Question: ");
        question = Console.ReadLine() ?? string.Empty;
    }
}

async Task ExecuteConversationAnalysis()
{
    var client = ConversationAnalysisClientFactory.Create();

    // Call the Language service model to get intent and entities
    var projectName = "Clock";
    var deploymentName = "production";

    Console.Write("Question: ");
    var question = Console.ReadLine() ?? string.Empty;

    while(question.ToLower() != "quit")
    {
        var data = new
        {
            analysisInput = new
            {
                conversationItem = new
                {
                    text = question,
                    id = "1",
                    participantId = "1",
                }
            },
            parameters = new
            {
                projectName,
                deploymentName,
                // Use Utf16CodeUnit for strings in .NET.
                stringIndexType = "Utf16CodeUnit",
            },
            kind = "Conversation",
        };
        // Send request
        Response response = await client.AnalyzeConversationAsync(RequestContent.Create(data));
        dynamic conversationalTaskResult = response.Content.ToDynamicFromJson(JsonPropertyNames.CamelCase);
        dynamic conversationPrediction = conversationalTaskResult.Result.Prediction;   
        var options = new JsonSerializerOptions { WriteIndented = true };
        Console.WriteLine(JsonSerializer.Serialize(conversationalTaskResult, options));
        Console.WriteLine("--------------------\n");
        Console.WriteLine(question);
        var topIntent = "";
        if (conversationPrediction.Intents[0].ConfidenceScore > 0.5)
        {
            topIntent = conversationPrediction.TopIntent;
            switch (topIntent)
            {
                case "GetTime":
                    var location = "local";           
                    // Check for a location entity
                    foreach (dynamic entity in conversationPrediction.Entities)
                    {
                        if (entity.Category == "Location")
                        {
                            //Console.WriteLine($"Location Confidence: {entity.ConfidenceScore}");
                            location = entity.Text;
                        }
                    }
                    // Get the time for the specified location
                    string timeResponse = location.GetTime();
                    Console.WriteLine(timeResponse);
                    break;
                case "GetDay":
                    var date = DateTime.Today.ToShortDateString();            
                    // Check for a Date entity
                    foreach (dynamic entity in conversationPrediction.Entities)
                    {
                        if (entity.Category == "Date")
                        {
                            //Console.WriteLine($"Location Confidence: {entity.ConfidenceScore}");
                            date = entity.Text;
                        }
                    }            
                    // Get the day for the specified date
                    string dayResponse = date.GetDay();
                    Console.WriteLine(dayResponse);
                    break;
                case "GetDate":
                    var day = DateTime.Today.DayOfWeek.ToString();
                    // Check for entities            
                    // Check for a Weekday entity
                    foreach (dynamic entity in conversationPrediction.Entities)
                    {
                        if (entity.Category == "Weekday")
                        {
                            //Console.WriteLine($"Location Confidence: {entity.ConfidenceScore}");
                            day = entity.Text;
                        }
                    }          
                    // Get the date for the specified day
                    string dateResponse = day.GetDate();
                    Console.WriteLine(dateResponse);
                    break;
                default:
                    // Some other intent (for example, "None") was predicted
                    Console.WriteLine("Try asking me for the time, the day, or the date.");
                    break;
            }
        }

        Console.Write("Question: ");
        question = Console.ReadLine() ?? string.Empty;
    }
}

async Task ExecuteClassifyText()
{
    var client = LanguageClientFactory.Create();

    var batchedDocuments = new List<string>();
                
    var folderPath = Path.GetFullPath("./resources/articles");  
    var folder = new DirectoryInfo(folderPath);
    var files = folder.GetFiles("*.txt");
    
    foreach (var file in files)
    {
        // Read the file contents
        StreamReader sr = file.OpenText();
        var text = sr.ReadToEnd();
        sr.Close();
        batchedDocuments.Add(text);
    }

    // Get Classifications
    ClassifyDocumentOperation operation = await client.SingleLabelClassifyAsync(WaitUntil.Completed, batchedDocuments, "ClassifyLab", "articles");

    int fileNo = 0;
    await foreach (ClassifyDocumentResultCollection documentsInPage in operation.Value)
    {
        foreach (ClassifyDocumentResult documentResult in documentsInPage)
        {
            Console.WriteLine(files[fileNo].Name);
            if (documentResult.HasError)
            {
                Console.WriteLine($"  Error!");
                Console.WriteLine($"  Document error code: {documentResult.Error.ErrorCode}");
                Console.WriteLine($"  Message: {documentResult.Error.Message}");
                continue;
            }

            Console.WriteLine($"  Predicted the following class:");
            Console.WriteLine();

            foreach (ClassificationCategory classification in documentResult.ClassificationCategories)
            {
                Console.WriteLine($"  Category: {classification.Category}");
                Console.WriteLine($"  Confidence score: {classification.ConfidenceScore}");
                Console.WriteLine();
            }
            fileNo++;
        }
    }
}

async Task ExecuteCustomNamedEntityRecognition()
{
    var client = LanguageClientFactory.Create();

    var batchedDocuments = new List<string>();
                
    var folderPath = Path.GetFullPath("./resources/ads");  
    var folder = new DirectoryInfo(folderPath);
    var files = folder.GetFiles("*.txt");
    
    foreach (var file in files)
    {
        // Read the file contents
        StreamReader sr = file.OpenText();
        var text = sr.ReadToEnd();
        sr.Close();
        batchedDocuments.Add(text);
    }

    // Extract entities
    RecognizeCustomEntitiesOperation operation = await client.RecognizeCustomEntitiesAsync(WaitUntil.Completed, batchedDocuments, "CustomEntityLab", "AdEntities");

    await foreach (RecognizeCustomEntitiesResultCollection documentsInPage in operation.Value)
    {
        foreach (RecognizeEntitiesResult documentResult in documentsInPage)
        {
            Console.WriteLine($"Result for \"{documentResult.Id}\":");
        if (documentResult.HasError)
        {
            Console.WriteLine($"  Error!");
            Console.WriteLine($"  Document error code: {documentResult.Error.ErrorCode}");
            Console.WriteLine($"  Message: {documentResult.Error.Message}");
            Console.WriteLine();
            continue;
        }

        Console.WriteLine($"  Recognized {documentResult.Entities.Count} entities:");

        foreach (CategorizedEntity entity in documentResult.Entities)
        {
            Console.WriteLine($"  Entity: {entity.Text}");
            Console.WriteLine($"  Category: {entity.Category}");
            Console.WriteLine($"  Offset: {entity.Offset}");
            Console.WriteLine($"  Length: {entity.Length}");
            Console.WriteLine($"  ConfidenceScore: {entity.ConfidenceScore}");
            Console.WriteLine($"  SubCategory: {entity.SubCategory}");
            Console.WriteLine();
        }

        Console.WriteLine();
    }
    }
}

async Task ExecuteTranslator()
{
    var client = TranslationClientFactory.Create();

    // Choose target language
    var languagesResponse = await client.GetLanguagesAsync(scope:"translation");
    GetLanguagesResult languages = languagesResponse.Value;
    Console.WriteLine($"{languages.Translation.Count} languages available.\n(See https://learn.microsoft.com/azure/ai-services/translator/language-support#translation)");
    Console.WriteLine("Enter a target language code for translation (for example, 'en'):");
    string targetLanguage = "xx";
    bool languageSupported = false;
    while (!languageSupported)
    {
        targetLanguage = Console.ReadLine();
        if (languages.Translation.ContainsKey(targetLanguage))
        {
            languageSupported = true;
        }
        else
        {
            Console.WriteLine($"{targetLanguage} is not a supported language.");
        }
    }

    // Translate text
    string inputText = "";
    while (inputText.ToLower() != "quit")
    {
        Console.WriteLine("Enter text to translate ('quit' to exit)");
        inputText = Console.ReadLine();
        if (inputText.ToLower() != "quit")
        {
            var translationResponse = await client.TranslateAsync(targetLanguage, inputText);
            IReadOnlyList<TranslatedTextItem> translations = translationResponse.Value;
            TranslatedTextItem translation = translations[0];
            string sourceLanguage = translation?.DetectedLanguage?.Language;
            Console.WriteLine($"'{inputText}' translated from {sourceLanguage} to {translation?.Translations[0].To} as '{translation?.Translations?[0]?.Text}'.");
        }
    } 
}