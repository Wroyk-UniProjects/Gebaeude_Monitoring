using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using BuildingMonitoringFunctionsapp.src.utils;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRooms
    {
        [FunctionName("getRooms")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms")]
        HttpRequest req,
        [Sql("select r.[id], " +
             "m.[temper], " +
             "m.[humid], " +
             "r.[name], " +
             "r.[global], " +
             "rc.[targetTemper], " +
             "rc.[targetHumid] " +
             "from measurement m "+
             "join room r " +
             "on m.roomId=r.id " +
             "join roomConfig rc " +
             "on r.id=rc.id",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "sqlconnectionstring")]
        IEnumerable<Room> rooms)
        {
            var connection_str = Environment.GetEnvironmentVariable("sqlconnectionstring");

            List<Measurement> l = new List<Measurement>();

            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                try
                {
                    foreach (Room room in rooms)
                    {
                        RoomConfig rc = RoomConfigsUtil.createRoomConfig(room.id, connection);
                        Measurement m = MeasurementUtil.createMeasurement(room.id, connection);
                        room.status = StatusUtil.GetStatus(m, rc, room.status);
                    }
                }
                catch (Exception es)
                {
                    return new BadRequestObjectResult(es);
                }
            }
            return new OkObjectResult(rooms);
        }
    }
}
