using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace gehcf1
{
    public class ServiceStatus
    {
        public string servicedesc { get; set; }
        public int servicestateid { get; set; }
    }

    public static class HttpTriggerAggregator
    {
        [FunctionName("HttpTriggerAggregator")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();

            // convert json to c# object
            ServiceStatus[] sso = JsonConvert.DeserializeObject<ServiceStatus[]>(requestBody);

            // group services by status
            var counts = sso.GroupBy(item => item.servicestateid);

            // build the output csv
            string csvOutput = "";
            string logOutput = "";
            var countOfOk = counts.FirstOrDefault(c => c.Key == 0);
            if (countOfOk != null)
            {
                csvOutput += $"OK, {countOfOk.Count()}\n";
                logOutput += $"{{OK: {countOfOk.Count()}}}\n";
            }

            var countOfWarning = counts.FirstOrDefault(c => c.Key == 1);
            if (countOfWarning != null)
            {
                csvOutput += $"Warning, {countOfWarning.Count()}\n";
                logOutput += $"{{Warning: {countOfWarning.Count()}}}\n";
            }

            var countOfCritical = counts.FirstOrDefault(c => c.Key == 2);
            if (countOfCritical != null)
            {
                csvOutput += $"Critical, {countOfCritical.Count()}\n";
                logOutput += $"{{Critical: {countOfCritical.Count()}}}\n";
            }

            // write the csv file to storage blob
            var conn = Utils.GetEnvironmentVariable("AzureWebJobsStorage");
            CloudStorageAccount acct;
            CloudStorageAccount.TryParse(conn, out acct);
            var cloudBlobClient = acct.CreateCloudBlobClient();
            var cloudContainer = cloudBlobClient.GetContainerReference("gehchotpath");
            CloudBlockBlob cloudBlockBlob = cloudContainer.GetBlockBlobReference("piechartblob.csv");
            cloudBlockBlob.UploadTextAsync(csvOutput).Wait();

            log.LogInformation(logOutput);

            

        }
    }
}
