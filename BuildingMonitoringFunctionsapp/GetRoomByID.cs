using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomByID
    {
        [FunctionName("getRoomByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{iD}/config")]
            HttpRequest req,
            [Sql("select r.[id], r.[name], r.[individual], 'OK' as status, m.[hum], m.[temp], rc.[targetTemp], rc.[targetHum] from dbo.rooms r " +
                "join roomConfig rc on r.[configId]=rc.[id] join measurements m on r.[id]=m.[roomId] where rc.[id] = @ID",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@ID={iD}",
                ConnectionStringSetting = "sqlconnectionstring")]
            IEnumerable<RoomConfig> RoomConfig)
        {
            return new JsonResult(RoomConfig);
        }
    }
}

// "select [Id], [order], [title], [url], [completed] from dbo.ToDo where [Priority] > @Priority"