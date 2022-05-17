using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomConfigByID
    {
        [FunctionName("getRoomConfigByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{iD}/config")]
            HttpRequest req,
             [Sql("select [roomId], [targetTemp], [targetHum],[updateRate],[uperToleranceT],[lowerToleranceT],[uperToleranceH], [lowerToleranceH] from roomConfig  "+
                  "where [roomId] = @ID",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@ID={iD}",
                ConnectionStringSetting = "sqlconnectionstring")]

        IEnumerable<RoomConfig>room)
        {
            return new OkObjectResult(room.FirstOrDefault());
        } 
 }
    }
