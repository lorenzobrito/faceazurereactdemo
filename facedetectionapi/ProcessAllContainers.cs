using apiface2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace facedetectionapi
{
   public class ProcessAllContainers
    {
      
        public async Task Process()
        {
            var blobs = new BlobStorage();
            var client = FaceApiAzure.Authenticate(FaceApiAzure.ENDPOINT, FaceApiAzure.SUBSCRIPTION_KEY);

            var containers =await blobs.getListContainers();
            foreach (var name in containers)
            {
               
                //   await uploadpicturesAsync(@"C:/Users/lorenzo/Pictures/arnold", name);

                //Console.WriteLine("Hello World!");
              
           
               
                var groupcreated = await FaceApiAzure.CreateIfnotexistIFaceClient(client, FaceApiAzure.personGroupId);

                Settings settings = new Settings();
                var list = (await settings.getListFiles(name)).ToList();
              

                var personFound = await FaceApiAzure.CreatePersonIfnotexist(client, name, FaceApiAzure.personGroupId);
                //   await PersonGroupSample.DeletePerson(client,personFound, personGroupId);
                await FaceApiAzure.FiltersUrlAsync(client, list, personFound, FaceApiAzure.personGroupId);
                if(list.Count()>0)
                await FaceApiAzure.AddGroup(client, list.ToList(), personFound, FaceApiAzure.personGroupId);
               
            }
            await FaceApiAzure.Train(client, FaceApiAzure.personGroupId);
        }
    }
}
