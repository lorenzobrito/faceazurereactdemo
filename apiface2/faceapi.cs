using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using apiface2;
using DtoModels;

namespace faceapifunctions
{
    public static class faceapi
    {


        [FunctionName("faceapi")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (req.Method.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    var copy = await new faceapilogic().getUser(req.Body);
                    copy.dataUri = await BlobStorage.UploadText(copy);
                    var user = new User(copy);    
                    var table = await Settings.CreateTableAsync();
                   
                    var result = await Settings.InsertOrMergeEntityAsync(table, user);
                    return new OkObjectResult(result);
                }
                catch (Exception ex)
                {

                   return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
           
            }
            string containerquery = req.Query["container"];
            if (!string.IsNullOrEmpty(containerquery))
            {
                return new OkObjectResult(containerquery);
            }
            var  containers = await new  BlobStorage().getListContainers();
           

            return new OkObjectResult(containers);
        }
    }
}
