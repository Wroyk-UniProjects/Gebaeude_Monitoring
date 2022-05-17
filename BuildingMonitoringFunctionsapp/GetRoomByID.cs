using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomByID
    {
        [FunctionName("getRoomByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{iD}")]
            HttpRequest req,
             [Sql("select r.[id], m.[date], m.[temp], m.[hum] , r.[name], r.[id] as 'roomId', r.[name], r.[individual], 'ok' as status, rc.[targetTemp], rc.[targetHum] from measurements m "+
                  "join rooms r on m.roomId=r.id join roomConfig rc on r.id=rc.roomId where date in (select max(date) from measurements group by roomId)" +
                  "and r.[id] = @ID",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@ID={iD}",
                ConnectionStringSetting = "sqlconnectionstring")]

       IEnumerable<Room> room)
        {
            return new OkObjectResult(room);
        }
    }
}

