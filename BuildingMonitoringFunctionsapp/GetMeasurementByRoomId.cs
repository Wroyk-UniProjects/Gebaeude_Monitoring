using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetMeasurementByRoomId
    {
        [FunctionName("getMeasurementByRoomId")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{iD}/measurement")]
            HttpRequest req,
            [Sql("select * from measurements where roomId=@ID",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@ID={iD}",
                ConnectionStringSetting = "sqlconnectionstring")]
            IEnumerable<Measurement> measurement)
        {
            return new OkObjectResult(measurement);
        }
      }
    }

