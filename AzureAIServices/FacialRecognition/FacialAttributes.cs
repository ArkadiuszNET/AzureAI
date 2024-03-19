namespace AzureAIServices.FacialRecognition;

using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;

public class FacialAttributes
{
    private readonly IFaceClient _client;
    private const string DetectionFaceModel = DetectionModel.Detection01;
    private const string RecognitionFaceModel = RecognitionModel.Recognition04;

    public FacialAttributes(IFaceClient client)
    {
        _client = client;
    }

    public Task<IList<DetectedFace>> SendWebImage(string imageUrl)
    {
        var faceAttributes = new List<FaceAttributeType>
        { 
            FaceAttributeType.Accessories,
            FaceAttributeType.Age,
            FaceAttributeType.Gender, 
            FaceAttributeType.Makeup,
            FaceAttributeType.FacialHair,
            FaceAttributeType.Mask,
            FaceAttributeType.Blur,
            FaceAttributeType.Emotion,
            FaceAttributeType.Exposure,
            FaceAttributeType.FacialHair,
            FaceAttributeType.Glasses,
            FaceAttributeType.Hair,
            FaceAttributeType.HeadPose,
        };

        return _client.Face.DetectWithUrlAsync(imageUrl, detectionModel: DetectionFaceModel,
            returnFaceAttributes: faceAttributes, recognitionModel: RecognitionFaceModel);
    }
}