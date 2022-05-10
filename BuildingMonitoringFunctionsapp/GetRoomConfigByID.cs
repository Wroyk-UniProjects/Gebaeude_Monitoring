using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomConfigByID
    {
        [FunctionName("getRoomConfigByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{iD}/config")]
            HttpRequest req,
            [Sql("select rc.[roomId], rc.[targetHum], rc.[uperToleranceH], rc.[lowerToleranceH], rc.[targetTemp], rc.[uperToleranceT], rc.[lowerToleranceT], rc.[updateRate]" +
                "from dbo.rooms r " +
                "join roomConfig rc on r.[configId]=rc.[id] " +
                "join measurements m on r.[id]=m.[roomId] " +
                "where r.[id] = @ID",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@ID={iD}",
                ConnectionStringSetting = "sqlconnectionstring")]
            IEnumerable<RoomConfig> roomConfig)
        {
            return new OkObjectResult(roomConfig);
        }
    }
}