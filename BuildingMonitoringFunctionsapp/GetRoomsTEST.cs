using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomsTEST
    {
        [FunctionName("getRoomsTEST")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "roomsTEST")]
        HttpRequest req,
        [Sql("select r.[id], r.[name], r.[individual], 'ok' as status, m.[hum], m.[temp], rc.[targetTemp], rc.[targetHum] from dbo.rooms r " +
            "join roomConfig rc on r.configId=rc.id join measurements m on r.id=m.roomId",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "sqldb_connection")]
        IEnumerable<Room> room1)
        {
            return new OkObjectResult(room1);
        }
    }
}
