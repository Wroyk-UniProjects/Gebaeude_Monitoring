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
             "r.[status],"+
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
            String sql_setStatus = "update room set [status]=@status where id=@roomID";

            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                try
                {
                    foreach (Room room in rooms)
                    {
                        SqlCommand cmd_setStatus = new SqlCommand(sql_setStatus, connection);

                        cmd_setStatus.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
                        cmd_setStatus.Parameters.Add("@status", System.Data.SqlDbType.VarChar);
                        cmd_setStatus.Parameters[0].Value = room.id;

                        RoomConfig rc = RoomConfigsUtil.createRoomConfig((room.global ? 0 : room.id), connection);
                        room.targetHumid = rc.targetHumid;
                        room.targetTemper = rc.targetTemper;
                        Measurement m = MeasurementUtil.createMeasurement(room.id, connection);
                        room.status = StatusUtil.GetStatus(m, rc, room.status);

                        cmd_setStatus.Parameters[1].Value = StatusUtil.GetStatus(m, rc, room.status);

                        connection.Open();
                        cmd_setStatus.ExecuteNonQueryAsync();
                        connection.Close();
                    }
                }
                catch (Exception es)
                {
                    return new BadRequestObjectResult(es);
                }

            }
            return new OkObjectResult(rooms); //200 zur�ckgeben
        }
    }
}
