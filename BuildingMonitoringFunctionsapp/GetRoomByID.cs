using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomByID
    {
        [FunctionName("getRoomByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{iD}")]
            HttpRequest req,
         [Sql("(select r.[id], m.[temper], m.[humid], r.[name], r.[global], " +
              "rc.[targetTemper], rc.[targetHumid] " +
              "from measurement m  " +
              "join room r on m.roomId=r.id " +
              "join roomConfig rc on r.id=rc.id " +
              "where r.global=0 and r.[id] = @ID)"+
              "union"+
              "(select r.[id], m.[temper], m.[humid], " +
              "r.[name], r.[global], " +
              "(select rc1.targetTemper from roomConfig rc1 where rc1.id=0),  " +
              "(select rc11.targetHumid from roomConfig rc11 where rc11.id=0) " +
              "from measurement m  join room r on m.roomId=r.id " +
              "join roomConfig rc on r.id=rc.id where r.global=1 and r.[id] = @ID)",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@ID={iD}",
                ConnectionStringSetting = "sqlconnectionstring")]
            IEnumerable<RoomDetail> room)
        {
            return new OkObjectResult(room.FirstOrDefault());
        }
    }
}

