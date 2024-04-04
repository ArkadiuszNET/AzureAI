using AzureAIServices.Options;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;

namespace AzureAIServices.ClientFactories
{
    public class SpeechClientFactory
    {
        public static SpeechSynthesizer Create()
        {
            var config = SpeechConfig.FromSubscription(AzureSettings.Speech.Key, AzureSettings.Speech.Region);
            config.SpeechSynthesisVoiceName = "pl-PL-MarekNeural";

            return new SpeechSynthesizer(config);
        }

        public static SpeechRecognizer CreateRecognizer()
        {
            var config = SpeechConfig.FromSubscription(AzureSettings.Speech.Key, AzureSettings.Speech.Region);
            config.SpeechSynthesisVoiceName = "pl-PL-MarekNeural";

            var sourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(["fr-FR","pl-PL","en-GB","de-DE"]); //at least the candidates for detection

            return new SpeechRecognizer(config, sourceLanguageConfig);
        }

        public static (SpeechSynthesizer, TranslationRecognizer) CreateTranslator()
        {
            var transaltionConfig = SpeechTranslationConfig.FromSubscription(AzureSettings.Speech.Key, AzureSettings.Speech.Region);
            transaltionConfig.SpeechRecognitionLanguage = "pl-PL";
            transaltionConfig.AddTargetLanguage("en-GB");

            var speechConfig = SpeechConfig.FromSubscription(AzureSettings.Speech.Key, AzureSettings.Speech.Region);
            speechConfig.SpeechSynthesisVoiceName = "en-GB-AbbiNeural";

            return (new SpeechSynthesizer(speechConfig), new TranslationRecognizer(transaltionConfig));
        }
    }
}