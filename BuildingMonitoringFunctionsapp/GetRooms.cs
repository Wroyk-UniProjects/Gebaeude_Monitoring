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
            List<RoomConfig> roomConfigs = new List<RoomConfig>();
            List<Measurement> measurements = new List<Measurement>();
            try
            {
                roomConfigs = RoomConfigsUtil.getRoomConfigList();
                measurements = MeasurementUtil.getMeasurementList();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            try
            {
                foreach (var room in rooms)
                {
                    try
                    {
                        foreach (var roomconfig in roomConfigs)
                        {
                            try
                            {
                                foreach (var measurement in measurements)
                                {
                                    try
                                    {
                                        if (room.id == roomconfig.id && roomconfig.id == measurement.roomId)
                                        {
                                            room.status = StatusUtil.GetStatus(measurement, roomconfig, room.status);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        return new BadRequestObjectResult(ex);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return new BadRequestObjectResult(ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new BadRequestObjectResult(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            return new OkObjectResult(rooms);
        }
    }
}

