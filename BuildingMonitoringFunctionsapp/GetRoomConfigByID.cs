
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomConfigByID
    {
        [FunctionName("getRoomConfigByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{roomID}/config")]
            HttpRequest req,
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
                 "r.configId=rc.id " +
                 "where rc.id=(select configId from room where id=@roomID)",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@roomID={roomID}",
                ConnectionStringSetting = "sqlconnectionstring")]
            IEnumerable<RoomConfig> roomConfig)
        {
            return new OkObjectResult(roomConfig.FirstOrDefault());
        }
    }
}