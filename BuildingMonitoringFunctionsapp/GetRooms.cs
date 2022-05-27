using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRooms
    {
        [FunctionName("getRooms")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms")]
        HttpRequest req,
        [Sql("select r.[id], m.[temp], m.[humid], r.[name], r.[individual], rc.[targetTemp], rc.[targetHum] from measurement m "+
            "join room r on m.roomId=r.id join roomConfig rc on r.id=rc.id",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "sqlconnectionstring")]
        IEnumerable<Room> rooms)
        {
            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");

            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                foreach (var room in rooms)
                {
                    try
                    {
                        //  Raumkonfiguration basierend auf der Raum-ID abrufen + RoomConfig-Objekt erzeugen
                        RoomConfig roomConfig = new RoomConfig(room.id, connection);
                        if (roomConfig != null)
                        {
                            //  Ist-Raumtemperatur is kleiner als Soll-Temperatur
                            if (room.temp.CompareTo(roomConfig.targetTemp + roomConfig.upperToleranceT) == -1)
                            {
                                room.status = "too low";
                            } //  Ist-Raumtemperatur is groeﬂer als Soll-Temperatur
                            else if (room.temp.CompareTo(roomConfig.targetTemp + roomConfig.upperToleranceT) == 1)
                            {
                                room.status = "too high";
                            } //  Ist-Raumtemperatur is gleich als Soll-Temperatur
                            else
                            {
                                room.status = "ok";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new BadRequestObjectResult(ex);
                    }
                }                
            }
            return new OkObjectResult(rooms);
        }
    }
}
