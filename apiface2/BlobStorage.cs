using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DtoModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace apiface2
{
  public  class BlobStorage
    {
        public async Task<IEnumerable<string>> getListContainers()
        {

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Settings.storageConnectionString);

            //create client
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            BlobContinuationToken continuationToken = null;
            var containers = new List<CloudBlobContainer>();

            do
            {
                ContainerResultSegment response = await cloudBlobClient.ListContainersSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                containers.AddRange(response.Results);

            } while (continuationToken != null);
            var containersname = containers.Select(t => t.Name).Where(t=>t!= "$web");
            return containersname;
        }
        public async Task<string> getUriFromBlobAsync(string container, string name)
        {
            //This will create the storage account to get the details of account.
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Settings.storageConnectionString);

            //create client
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            //Get a container
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(container);

            //From here we will get the URL of file available in Blob Storage.
            var blob1 = cloudBlobContainer.GetBlockBlobReference(name);
            string FileURL = blob1.Uri.AbsoluteUri;
            var blobPolicy = new SharedAccessBlobPolicy();
            blobPolicy.SharedAccessExpiryTime = DateTime.Now.AddYears(10);
            blobPolicy.Permissions = SharedAccessBlobPermissions.Read;

            var blobPermissions = new BlobContainerPermissions();
            blobPermissions.SharedAccessPolicies.Add("ReadBlobPolicy", blobPolicy);
            blobPermissions.PublicAccess = BlobContainerPublicAccessType.Off;

            await cloudBlobContainer.SetPermissionsAsync(blobPermissions);
            var sasToken = cloudBlobContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "ReadBlobPolicy");
            var blob = cloudBlobContainer.GetBlockBlobReference(name);
            var blobUri = blob.Uri;
            var secureUrl = blobUri.AbsoluteUri + sasToken;

            return secureUrl;
            
        }
        public static async Task<string> UploadText(User entity)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(Settings.storageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(entity.name);
           
            if (!containerClient.Exists())
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(entity.name, PublicAccessType.BlobContainer);
               
            }
            //Get a container
            string container = Guid.NewGuid().ToString();
            BlobClient blobClient = containerClient.GetBlobClient(container);
            int index = entity.dataUri.IndexOf(",")+1;
            string base64 = entity.dataUri.Substring(index);
            var bytes = Convert.FromBase64String(base64);

            using (var stream = new MemoryStream(bytes))
            {
                //cblob.UploadFromStream(stream);
                await blobClient.UploadAsync(stream);
            }
            
            
            string FileURL =await new BlobStorage().getUriFromBlobAsync(entity.name, container);

            if (string.IsNullOrEmpty(FileURL))
            {
                throw new Exception("FileURL is empty");
            }
            return FileURL;



        }
    }
}
