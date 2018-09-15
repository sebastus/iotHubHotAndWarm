using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace testAggregator
{
    public class ServiceStatus
    {
        public string servicedesc { get; set; }
        public int servicestateid { get; set; }
    }

    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json");

            Configuration = builder.Build();

            var fileName = Configuration["AppSettings:testFile"];

            string requestBody = new StreamReader(fileName).ReadToEnd();

            // convert json to c# object
            ServiceStatus[] sso = JsonConvert.DeserializeObject<ServiceStatus[]>(requestBody);

            //foreach (var ss in sso)
            //{
            //    Console.WriteLine($"service name: {ss.servicedesc}, service status: {ss.servicestateid}");
            //}

            // sum up count of service by status
            var counts = sso.GroupBy(item => item.servicestateid);

            string csvOutput = "";
            var countOfOk = counts.FirstOrDefault(c => c.Key == 0);
            if (countOfOk != null)
            {
                csvOutput += $"OK, {countOfOk.Count()}\n";
            }

            var countOfWarning = counts.FirstOrDefault(c => c.Key == 1);
            if (countOfWarning != null)
            {
                csvOutput += $"Warning, {countOfWarning.Count()}\n";
            }

            var countOfCritical = counts.FirstOrDefault(c => c.Key == 2);
            if (countOfCritical != null)
            {
                csvOutput += $"Critical, {countOfCritical.Count()}\n";
            }

            // write the csv file to storage blob
            var conn = Configuration["AppSettings:storageAcctConnection"];
            CloudStorageAccount acct;
            CloudStorageAccount.TryParse(conn, out acct);
            var cloudBlobClient = acct.CreateCloudBlobClient();
            var cloudContainer = cloudBlobClient.GetContainerReference("gehchotpath");
            CloudBlockBlob cloudBlockBlob = cloudContainer.GetBlockBlobReference("piechartblob.csv");
            cloudBlockBlob.UploadTextAsync(csvOutput).Wait();

            Console.ReadKey();
        }

        public static string GetEnvironmentVariable(string name)
        {
            var result = System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.Process);
            if (result == null)
                return "";

            return result;
        }
    }
}
