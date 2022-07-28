
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using BuildingMonitoringFunctionsapp.src.utils;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomConfigByID
    {
        [FunctionName("getRoomConfigByID")]
        public static IActionResult Run(

            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{roomID}/config")]
            HttpRequest req, int roomID,
            [Sql("select " +
                 "r.id," +
                 "rc.[targetTemper]," +
                 "rc.[targetHumid]," +
                 "rc.[updateRate]," +
                 "rc.[upperToleranceTemper]," +
                 "rc.[lowerToleranceTemper]," +
                 "rc.[upperToleranceHumid]," +
                 "rc.[lowerToleranceHumid] " +
                 "from roomConfig  " +
                 "rc join room r on " +
                 "r.id=rc.id " +
                 "where rc.id=@roomID",

                CommandType = System.Data.CommandType.Text,
                Parameters = "@roomID={roomID}",
                ConnectionStringSetting = "sqlconnectionstring")]
            IEnumerable<RoomConfig> roomConfig)
        {
            try
            {
                var connection_str = Environment.GetEnvironmentVariable("sqlconnectionstring");

                using (SqlConnection connection = new SqlConnection(connection_str))
                {
                    SqlCommand isGlobal = new SqlCommand("SELECT global FROM room where id = @roomID", connection);
                    isGlobal.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
                    isGlobal.Parameters[0].Value = roomID;
                    connection.Open();
                    SqlDataReader reader = isGlobal.ExecuteReader();
                    reader.Read();
                    bool globalBool = reader.GetBoolean(reader.GetOrdinal("global"));
                    connection.Close();
                    if (globalBool)
                    {
                        RoomConfig rc = RoomConfigsUtil.createRoomConfig(0, connection);
                        rc.id = roomID;
                        return new OkObjectResult(rc);
                    };
                    return new OkObjectResult(roomConfig.FirstOrDefault());
                }
            }
            catch (Exception es)
            {
                return new BadRequestObjectResult(es);
            }
        }
    }
}