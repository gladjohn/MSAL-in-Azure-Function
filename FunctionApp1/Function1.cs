using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Identity.Client;
using System.Runtime.InteropServices;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            IConfidentialClientApplication app = null;

            const string ClientId = "ClientID";
            const string resourceId = "https://resource.com";
            const string authority = "https://login.microsoftonline.com/72f988bf-86f1-41af-91ab-2d7cd011db47";

            log.LogInformation("C# HTTP trigger function processed a request.");

            if (app == null)
            {
                app = ConfidentialClientApplicationBuilder.Create(ClientId)
                .WithClientSecret("Secret")
                .WithAuthority(authority)
                .Build();
            }

            var authResult = await app.AcquireTokenForClient(new[] { $"{resourceId}/.default" })
                                .ExecuteAsync()
                                .ConfigureAwait(false);

            string responseMessage = $"Access Token :  {authResult.AccessToken}. OS Description : { RuntimeInformation.OSDescription } This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
