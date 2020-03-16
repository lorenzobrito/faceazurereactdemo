using apiface2;
using DtoModel;
using facedetectionapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace facedemoconsoletest
{
    class Program
    {
        static async System.Threading.Tasks.Task uploadpicturesAsync(string directory,string name)
        {
            var upload = new UploadPictures();
            var text = upload.fromDirectory(directory).ToList();
            var list = new List<UserRequest>();
            var httpclient = new HttpClient();

            foreach (var item in text)
            {
                var user = new UserRequest()
                {
                    name = name,
                    dataUri = "," + upload.ConvertImageto64(item)
                };
                var response = await httpclient.PostAsJsonAsync<UserRequest>("https://apifacelorbrito.azurewebsites.net/api/faceapi?code=LSPc9n6WBiC33f2u9GFlvrygmfLEVcw4OJgeDVaYMPJIBig6yKv6yA==", user);
                Console.WriteLine(response.StatusCode);
            }
        }
        static async System.Threading.Tasks.Task Main(string[] args)
        {


            var faceapi = new FaceApiAzure();
            var client = FaceApiAzure.Authenticate(FaceApiAzure.ENDPOINT, FaceApiAzure.SUBSCRIPTION_KEY);

            string personGroupId = "apifacegroup";
            await FaceApiAzure.Train(client, personGroupId);
            Console.ReadKey();
        }
        static async System.Threading.Tasks.Task ProcessAll()
        {
            var process = new ProcessAllContainers();
            try
            {
                await  process.Process();
            }
            catch (Exception ex)
            {

                throw;
            }
          
            Console.ReadKey();
        }
        static async System.Threading.Tasks.Task Upload()
        {
            string name = "arnoldschwarzenegger";
            await uploadpicturesAsync(@"C:/Users/lorenzo/Pictures/arnold", name);
            name= "tomcruise";
            await uploadpicturesAsync(@"C:/Users/lorenzo/Pictures/tomcruise", name);
            Console.ReadKey();
        }
        static async System.Threading.Tasks.Task Processone(string[] args)
        {
            string name = "arnoldschwarzenegger";
        //   await uploadpicturesAsync(@"C:/Users/lorenzo/Pictures/arnold", name);
        
            //Console.WriteLine("Hello World!");
            var faceapi = new FaceApiAzure();
            var client = FaceApiAzure.Authenticate(FaceApiAzure.ENDPOINT, FaceApiAzure.SUBSCRIPTION_KEY);
            
            string personGroupId = "apifacegroup";
            var groupcreated =await FaceApiAzure.CreateIfnotexistIFaceClient(client, personGroupId);
   
            Settings settings = new Settings();
            var list =(await settings.getListFiles(name)).ToList();
            
            
            var personFound =await FaceApiAzure.CreatePersonIfnotexist(client, name, personGroupId);
            //   await PersonGroupSample.DeletePerson(client,personFound, personGroupId);
            await FaceApiAzure.FiltersUrlAsync(client, list, personFound, personGroupId);
            if(list.Count>0)
              await FaceApiAzure.AddGroup(client, list.ToList(), personFound, personGroupId);
           

        }
    }
}
