
//    //test um etwas in roomconfig zu schreiben

using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

using System;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace FunctionApp2
{
    public class updateRoomConfig
    {
        private readonly ILogger<updateRoomConfig> _logger;

        public updateRoomConfig(ILogger<updateRoomConfig> log)
        {
            _logger = log;
        }

        [FunctionName("updateRoomConfig")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rooms/{iD}/roomConfig")]
            HttpRequest req,
             [Sql("update roomconfig set [targetTemp]=100, [targetHum]=1, [updateRate]=1, [uperToleranceT]=1," +
            "[uperToleranceH]=1,  [lowerToleranceT]=1, [lowerToleranceH]=1, " +
            "[roomId]= 2 where [roomId] = @ID",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@ID={iD}",
                ConnectionStringSetting = "sqlconnectionstring")]

       IEnumerable<updateRoomConfig> room)
        {
            return new OkObjectResult(room);
        }
    }
}

























//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using BuildingMonitoringFunctionsapp;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;

//namespace FunctionApp2
//{
//    public class updateRoomConfig
//    {
//        // create a new ToDoItem from body object
//        // uses output binding to insert new item into ToDo table
//        [FunctionName("PostToDo")]

//        public static IActionResult Run(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rooms/{iD}/measurement")]
//            HttpRequest req,
//            [Sql("insert into measurements ([roomId], [temp], [hum])  values( @ID,5,  2) ",
//                CommandType = System.Data.CommandType.Text,
//                Parameters = "@ID={iD}",
//                ConnectionStringSetting = "sqlconnectionstring")]
//            IEnumerable<Measurement> measurement)
//        {
//            return new OkObjectResult(measurement);
//        }
//        //public static async Task<IActionResult> Run(
//        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostFunction")] HttpRequest req,
//        //    ILogger log,
//        //    [Sql("dbo.measurements", ConnectionStringSetting = "sqlconnectionstring")] IAsyncCollector<Measurement> measurements)
//        //{
//        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//        //    Measurement measurement = JsonConvert.DeserializeObject<Measurement>(requestBody);



//        //    await measurements.AddAsync(measurement);
//        //    await measurements.FlushAsync();
//        //    List<Measurement> measurementList = new List<Measurement> { measurement };

//        //    return new OkObjectResult(measurementList);
//        //}
//    } 
//}


