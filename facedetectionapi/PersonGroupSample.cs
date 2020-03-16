using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace facedetectionapi
{
    public class PersonGroupSample
    {
        //source https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/csharp-sdk

        // Used for all examples.
        // URL for the images.
        const string IMAGE_BASE_URL = "https://csdx.blob.core.windows.net/resources/Face/Images/";
        // Used in the Detect Faces and Verify examples.
        // Recognition model 2 is used for feature extraction, use 1 to simply recognize/detect a face. 
        // However, the API calls to Detection that are used with Verify, Find Similar, or Identify must share the same recognition model.
        const string RECOGNITION_MODEL2 = RecognitionModel.Recognition02;
        const string RECOGNITION_MODEL1 = RecognitionModel.Recognition01;
        static string sourcePersonGroup = null;
        // Create a dictionary for all your images, grouping similar ones under the same key.
       
        // A group photo that includes some of the persons you seek to identify from your dictionary.
        string sourceImageFileName = "identification1.jpg";

        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string RECOGNITION_MODEL1)
        {
            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: RECOGNITION_MODEL1);
            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(url)}`");
            return detectedFaces.ToList();
        }

        public static async Task CreateSampleProjectAsync(IFaceClient client,string url, string personGroupId)
        {
            Dictionary<string, string[]> personDictionary =
           new Dictionary<string, string[]>
               {
                 { "Family1-Dad", new[] { "Family1-Dad1.jpg", "Family1-Dad2.jpg" } },
                 { "Family1-Mom", new[] { "Family1-Mom1.jpg", "Family1-Mom2.jpg" } },
                 { "Family1-Son", new[] { "Family1-Son1.jpg", "Family1-Son2.jpg" } },
                 { "Family1-Daughter", new[] { "Family1-Daughter1.jpg", "Family1-Daughter2.jpg" } },
                 { "Family2-Lady", new[] { "Family2-Lady1.jpg", "Family2-Lady2.jpg" } },
                 { "Family2-Man", new[] { "Family2-Man1.jpg", "Family2-Man2.jpg" } }
               };
           // string personGroupId = Guid.NewGuid().ToString();
          //  sourcePersonGroup = personGroupId; // This is solely for the snapshot operations example
            Console.WriteLine($"Create a person group ({personGroupId}).");
            await client.PersonGroup.CreateAsync(personGroupId, personGroupId, recognitionModel: PersonGroupSample.RECOGNITION_MODEL1);
            // The similar faces will be grouped into a single person group person.
            foreach (var groupedFace in personDictionary.Keys)
            {
                // Limit TPS
                await Task.Delay(250);
                Person person = await client.PersonGroupPerson.CreateAsync(personGroupId: personGroupId, name: groupedFace);
                Console.WriteLine($"Create a person group person '{groupedFace}'.");

                // Add face to the person group person.
                foreach (var similarImage in personDictionary[groupedFace])
                {
                    Console.WriteLine($"Add face to the person group person({groupedFace}) from image `{similarImage}`");
                    PersistedFace face = await client.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId, person.PersonId,
                        $"{url}{similarImage}", similarImage);
                }
            }
            Console.WriteLine();
            Console.WriteLine($"Train person group {personGroupId}.");
            await client.PersonGroup.TrainAsync(personGroupId);

            // Wait until the training is completed.
            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await client.PersonGroup.GetTrainingStatusAsync(personGroupId);
                Console.WriteLine($"Training status: {trainingStatus.Status}.");
                if (trainingStatus.Status == TrainingStatusType.Succeeded) { break; }
            }

        }
      
       
      
        public static async Task IdentifyAsyncSample(IFaceClient client, string url, string sourceImageFileName, string personGroupId)
        {
            List<Guid> sourceFaceIds = new List<Guid>();
            // Detect faces from source image url.
            List<DetectedFace> detectedFaces = await DetectFaceRecognize(client, $"{url}{sourceImageFileName}", PersonGroupSample.RECOGNITION_MODEL1);

            // Add detected faceId to sourceFaceIds.
            foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }
            var identifyResults = await client.Face.IdentifyAsync(sourceFaceIds, personGroupId);

            foreach (var identifyResult in identifyResults)
            {
                Person person = await client.PersonGroupPerson.GetAsync(personGroupId, identifyResult.Candidates[0].PersonId);
                Console.WriteLine($"Person '{person.Name}' is identified for face in: {sourceImageFileName} - {identifyResult.FaceId}," +
                    $" confidence: {identifyResult.Candidates[0].Confidence}.");
            }
            Console.WriteLine();
        }
      
    
    }
}
