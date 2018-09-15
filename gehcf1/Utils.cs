using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;

namespace gehcf1
{
    public class Utils
    {
        public static string GetEnvironmentVariable(string name)
        {
            var result = System.Environment.GetEnvironmentVariable(name, System.EnvironmentVariableTarget.Process);
            if (result == null)
                return "";

            return result;
        }

        public static string InsertServiceRecord(string myQueueItem, string conn, ILogger log)
        {
            ServiceMessageObject smo = JsonConvert.DeserializeObject<ServiceMessageObject>(myQueueItem);
            log.LogInformation($"C# Queue trigger function processed new service message from host: {smo.HOSTNAME}");

            using (var connection = new SqlConnection(conn))
            {
                connection.Open();

                var sql = "";
                sql += $"INSERT INTO service (id";
                sql += $", TIMET";
                sql += $", [TimeT_datetime]";
                sql += $", HOSTNAME";
                sql += $", SERVICEDESC";
                sql += $", SERVICESTATEID";
                sql += $", SERVICEOUTPUT";
                sql += $", SERVICEPERFDATA";
                sql += $", LONGSERVICEOUTPUT";
                sql += $", start_time";
                sql += $", end_time";
                sql += $", latency";
                sql += $", deviceId";
                sql += $") VALUES ";
                sql += $"('{Guid.NewGuid()}'";
                sql += $", {smo.TIMET}";
                sql += $", dateadd(S, {smo.TIMET}, '1970-01-01')";
                sql += $", '{smo.HOSTNAME}'";
                sql += $", '{smo.SERVICEDESC}'";
                sql += $", {smo.SERVICESTATEID}";
                sql += $", '{smo.SERVICEOUTPUT}'";
                sql += $", '{smo.LONGSERVICEOUTPUT}'";
                sql += $", '{smo.SERVICEPERFDATA}'";
                sql += $", {smo.start_time}";
                sql += $", {smo.end_time}";
                sql += $", {(smo.latency==null?0: Convert.ToDouble(smo.latency))}";
                sql += $", '{smo.deviceId}'";
                sql += ") ";

                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.LogInformation($"Error executing query, sql = {sql}");
                }

                connection.Close();
            }
            return UnixTimeStampToDateTime(smo.TIMET).ToString("yyyyMMddHHmm");
        }

        public static string InsertHostRecord(string myQueueItem, string conn, ILogger log)
        {
            HostMessageObject hmo = JsonConvert.DeserializeObject<HostMessageObject>(myQueueItem);
            log.LogInformation($"C# Queue trigger function processed new host message from host: {hmo.HOSTNAME}");

            using (var connection = new SqlConnection(conn))
            {
                connection.Open();

                var sql = "";
                sql += $"INSERT INTO host (id";
                sql += $", TIMET";
                sql += $", [TimeT_datetime]";
                sql += $", HOSTNAME";
                sql += $", HOSTSTATEID";
                sql += $", HOSTOUTPUT";
                sql += $", start_time";
                sql += $", end_time";
                sql += $", latency";
                sql += $", deviceId";
                sql += $") VALUES (";
                sql += $"'{Guid.NewGuid()}'";
                sql += $", {hmo.TIMET}";
                sql += $", dateadd(S, {hmo.TIMET}, '1970-01-01')";
                sql += $", '{hmo.HOSTNAME}'";
                sql += $", {hmo.HOSTSTATEID}";
                sql += $", '{hmo.HOSTOUTPUT}'";
                sql += $", {hmo.start_time}";
                sql += $", {hmo.end_time}";
                sql += $", {(hmo.latency == null ? 0 : Convert.ToDouble(hmo.latency))}";
                sql += $", '{hmo.deviceId}'";
                sql += ") ";

                var cmd = connection.CreateCommand();
                cmd.CommandText = sql;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.LogInformation($"Error executing query, sql = {sql}");
                }


                connection.Close();
            }
            return UnixTimeStampToDateTime(hmo.TIMET).ToString("yyyyMMddHHmm");
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}