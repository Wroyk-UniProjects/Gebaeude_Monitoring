using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetGlobalConfig
    {
        [FunctionName("getGlobalConfig")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/config")]
            HttpRequest req,
            [Sql(" select [targetTemper],[targetHumid],[updateRate],[upperToleranceTemper],[lowerToleranceTemper]," +
            "[upperToleranceHumid],[lowerToleranceHumid] from roomConfig " +
            "where id=0;",
                CommandType = System.Data.CommandType.Text,
                ConnectionStringSetting = "sqlconnectionstring")]
            IEnumerable<RoomConfig> roomConfig)
        {
            return new OkObjectResult(roomConfig.FirstOrDefault());
        }
    }
}

