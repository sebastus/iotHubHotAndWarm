using System;
using System.Collections.Generic;
using System.Text;

namespace gehcf1
{
    public class IoTHubSvc
    {
        public string MessageId { get; set; }
        public string CorrelationId { get; set; }
        public string ConnectionDeviceId { get; set; }
        public string ConnectionDeviceGenerationId { get; set; }
        public string EnqueuedTime { get; set; }
        public string StreamId { get; set; }
    }

    public class ServiceMessageObject
    {
        public string TYPE { get; set; }
        public int TIMET { get; set; }
        public string HOSTNAME { get; set; }
        public string SERVICEDESC { get; set; }
        public int SERVICESTATEID { get; set; }
        public string SERVICEOUTPUT { get; set; }
        public string SERVICEPERFDATA { get; set; }
        public string LONGSERVICEOUTPUT { get; set; }
        public int start_time { get; set; }
        public int end_time { get; set; }
        public string latency { get; set; }
        public string deviceId { get; set; }
        public string EventProcessedUtcTime { get; set; }
        public int PartitionId { get; set; }
        public string EventEnqueuedUtcTime { get; set; }
        public IoTHubSvc IoTHub { get; set; }
    }

    public class IoTHubHost
    {
        public string MessageId { get; set; }
        public string CorrelationId { get; set; }
        public string ConnectionDeviceId { get; set; }
        public string ConnectionDeviceGenerationId { get; set; }
        public string EnqueuedTime { get; set; }
        public string StreamId { get; set; }
    }

    public class HostMessageObject
    {
        public string TYPE { get; set; }
        public int TIMET { get; set; }
        public string HOSTNAME { get; set; }
        public int HOSTSTATEID { get; set; }
        public string HOSTOUTPUT { get; set; }
        public int start_time { get; set; }
        public int end_time { get; set; }
        public string latency { get; set; }
        public string deviceId { get; set; }
        public string EventProcessedUtcTime { get; set; }
        public int PartitionId { get; set; }
        public string EventEnqueuedUtcTime { get; set; }
        public IoTHubHost IoTHub { get; set; }
    }
}
