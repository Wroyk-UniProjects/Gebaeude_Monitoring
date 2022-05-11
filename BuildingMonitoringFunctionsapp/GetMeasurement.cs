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
    public class GetMeasurements
    {
        [FunctionName("getMeasurements")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "measurements")]
        HttpRequest req,
        [Sql("select [id] from measurements",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "sqldb_connection")]
        IEnumerable<Measurement> measurements)
        {
            return new OkObjectResult(measurements);
        }
     }
}

