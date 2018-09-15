using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO;
using System;

namespace gehcf1
{
    public static class gehcIoTHub
    {
        [FunctionName("gehcIoTHub")]
        // [Singleton]
        public static void Run([EventHubTrigger("%iotHubName%", Connection = "iotHubConnection")]            
            string myEventHubMessage
            , [EventHub("%notifyHubName%", Connection = "notifyHubConnection")]out string notificationMessage
            , [EventHub("%hotPathHubName%", Connection = "hotPathHubConnection")]out string hotPathMessage
            //, [Blob("satellite-raw/{$return}", FileAccess.Write, Connection = "AzureWebJobsStorage")] out string rawInputMessage
            //, Binder blobOutputBinder
            , ILogger log)
        {
//            string name = "";
            var conn = Utils.GetEnvironmentVariable("sqlConnection");

            JObject jo = JObject.Parse(myEventHubMessage);
            var type = (string)jo["TYPE"];

            if (type == "HOST")
            {
                var t = Utils.InsertHostRecord(myEventHubMessage, conn, log);
//                name = $"h-event-{t}";
            }
            else if (type == "SERVICE")
            {
                var t = Utils.InsertServiceRecord(myEventHubMessage, conn, log);
//                name = $"s-event-{t}";
            }
            else if (type == "NOTIFICATION")
            {
//                name = $"n-event-{DateTime.Now.ToString("yyyyMMddHHmm")}";
                notificationMessage = myEventHubMessage;
                hotPathMessage = null;
                return;
            }

            notificationMessage = null;
            hotPathMessage = myEventHubMessage;

            // log the input
            //using (var writer = blobOutputBinder.Bind<TextWriter>(
            //    new BlobAttribute($"satellite-raw/{name}")))
            //{
            //    writer.Write(myEventHubMessage);
            //}
        }
    }
}


