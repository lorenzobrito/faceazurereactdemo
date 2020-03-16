using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DtoModels;
using faceapifunctions;
using facedetectionapi;

namespace apiface2
{
    public static class faceanalyze
    {
        [FunctionName("faceanalyze")]
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
                    var client = FaceApiAzure.Authenticate(FaceApiAzure.ENDPOINT, FaceApiAzure.SUBSCRIPTION_KEY);

                  var resultados=  await FaceApiAzure.IdentifyAsync(client, copy.dataUri, FaceApiAzure.personGroupId);
                        
                    return new OkObjectResult(resultados);
                }
                catch (Exception ex)
                {

                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

            }
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
