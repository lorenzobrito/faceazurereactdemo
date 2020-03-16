using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace facedetectionapi
{
  public  class FaceApiAzure
    {
       public  string SUBSCRIPTIONKEY = Environment.GetEnvironmentVariable("FACE_AZURE_SUBSCRIPTION_KEY");
       public static string SUBSCRIPTION_KEY
        {
            get {

                return new FaceApiAzure().SUBSCRIPTIONKEY;
            }

        }
        public  string ENDPOINT_ = Environment.GetEnvironmentVariable("FACE_AZURE_ENDPOINT");
        public static string ENDPOINT
        {
            get
            {

                return new FaceApiAzure().ENDPOINT_;
            }

        }
        public const string IMAGE_BASE_URL = "https://csdx.blob.core.windows.net/resources/Face/Images/";
        // Used in the Detect Faces and Verify examples.
        // Recognition model 2 is used for feature extraction, use 1 to simply recognize/detect a face. 
        // However, the API calls to Detection that are used with Verify, Find Similar, or Identify must share the same recognition model.
        public const string RECOGNITION_MODEL2 = RecognitionModel.Recognition02;
        public const string RECOGNITION_MODEL1 = RecognitionModel.Recognition01;
       public const string personGroupId = "apifacegroup";


        /* 
 * DETECT FACES
 * Detects features from faces and IDs them.
 */
        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string RECOGNITION_MODEL1)
        {
            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: RECOGNITION_MODEL1);
            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(url)}`");
            return detectedFaces.ToList();
        }

        public static async Task DetectFaceExtract(IFaceClient client, string url, string recognitionModel)
        {
            Console.WriteLine("========DETECT FACES========");
            Console.WriteLine();
            
            // Create a list of images
            List<string> imageFileNames = new List<string>
                    {
                        "detection1.jpg",    // single female with glasses
                        // "detection2.jpg", // (optional: single man)
                        // "detection3.jpg", // (optional: single male construction worker)
                        // "detection4.jpg", // (optional: 3 people at cafe, 1 is blurred)
                        "detection5.jpg",    // family, woman child man
                        "detection6.jpg"     // elderly couple, male female
                    };

            
              
            foreach (var imageFileName in imageFileNames)
            {
                IList<DetectedFace> detectedFaces;

                // Detect faces with all attributes from image url.
                detectedFaces = await client.Face.DetectWithUrlAsync($"{url}{imageFileName}",
                        returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                FaceAttributeType.Blur, FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.FacialHair,
                FaceAttributeType.Gender, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.HeadPose,
                FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion, FaceAttributeType.Smile },
                        recognitionModel: recognitionModel);
                Console.WriteLine($"{detectedFaces.Count} face(s)");
            }


               
            
        }
        public static async Task FiltersUrlAsync(IFaceClient client, List<string> urls, Person Personname, string personGroupId)
        {

            foreach (var PersistedFaceIds in Personname.PersistedFaceIds)
            {
                var faceitem = await client.PersonGroupPerson.GetFaceAsync(personGroupId, Personname.PersonId, PersistedFaceIds);
                urls.RemoveAll(t => t.Split('?')[0].Split('/')[4] == faceitem.UserData);
            }

        }
        public static async Task<bool> CreateIfnotexistIFaceClient(IFaceClient client, string personGroupId)
        {
            var listagrupo = await client.PersonGroup.ListAsync();

            if (listagrupo.Where(t => t.PersonGroupId.Equals(personGroupId)).Count() == 0)
            {
                Console.WriteLine($"Create a person group ({personGroupId}).");
                await client.PersonGroup.CreateAsync(personGroupId, personGroupId, recognitionModel: FaceApiAzure.RECOGNITION_MODEL1);
                return true;
            }
            else
            {
                return true;
            }

            return false;
        }

        public static async Task<Person> CreatePersonIfnotexist(IFaceClient client, string Personname, string personGroupId)
        {

            var lista = await client.PersonGroupPerson.ListAsync(personGroupId);
            var personFound = lista.FirstOrDefault(t => t.Name == Personname);
            if (personFound != null)
            {
                return personFound;
            }

            Person person = await client.PersonGroupPerson.CreateAsync(personGroupId: personGroupId, name: Personname);
            return person;
        }
        public static async Task DeletePerson(IFaceClient client, Person person, string personGroupId)
        {
            await client.PersonGroupPerson.DeleteAsync(personGroupId: personGroupId, personId: person.PersonId);

        }
     
        public static async Task<List<string>> IdentifyAsync(IFaceClient client, string url, string personGroupId)
        {
            List<Guid> sourceFaceIds = new List<Guid>();
            // Detect faces from source image url.
            List<DetectedFace> detectedFaces = await DetectFaceRecognize(client, url, FaceApiAzure.RECOGNITION_MODEL1);
            var results = new List<string>();
            // Add detected faceId to sourceFaceIds.
            foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }
            var identifyResults = await client.Face.IdentifyAsync(sourceFaceIds, personGroupId);

            foreach (var identifyResult in identifyResults)
            {
                if (identifyResult.Candidates.Count > 0)
                {
                    Person person = await client.PersonGroupPerson.GetAsync(personGroupId, identifyResult.Candidates[0].PersonId);
                    var result = $"Person '{person.Name}' is identified for face in:  - {identifyResult.FaceId}," +
                        $" confidence: {identifyResult.Candidates[0].Confidence}.";
                    Console.WriteLine(result);
                    results.Add(result);
                }
            }
            return results;
        }
        public static async Task AddGroup(IFaceClient client, List<string> urls, Person Personname, string personGroupId)
        {
            await Task.Delay(250);
          

            foreach (var url in urls)
            {
                // Limit TPS
                await Task.Delay(250);
                string userddata = url.Split('?')[0].Split('/')[4];

                PersistedFace face = await client.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId, Personname.PersonId,
                       url, userddata);
                // Add face to the person group person.

            }
          

        }
        public static async Task Train(IFaceClient client, string personGroupId)
        {
            
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
        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }
        public override string ToString()
        {
            return ENDPOINT;
        }
    }
}
