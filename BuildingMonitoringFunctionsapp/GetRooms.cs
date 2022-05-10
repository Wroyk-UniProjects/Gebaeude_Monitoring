using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRooms
    {
        [FunctionName("getRooms")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms")]
        HttpRequest req,
        //[Sql("select r.[id], r.[name], r.[individual], 'ok' as status, m.[hum], m.[temp], rc.[targetTemp], rc.[targetHum] from dbo.rooms r " +
        //    "join roomConfig rc on r.configId=rc.id join measurements m on r.id=m.roomId",
        //    CommandType = System.Data.CommandType.Text,
        //    ConnectionStringSetting = "sqlconnectionstring")]
        [Sql("select * from dbo.rooms",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "sqlconnectionstring")]
        IEnumerable<Room> Rooms)
        {
            return new JsonResult(Rooms);
        }
    }
}
