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
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";



            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                //  var text = "update roomConfig set "+name+" = 600 where roomId=2;";
                // var text = "update roomConfig set [targetHum] = 500 where roomId= " + name + ";";
                var text = "insert into roomConfig ([targetTemp],[targetHum], [updateRate], [roomId])  values( 1, 2,55,  2)";


                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    _logger.LogInformation($"{rows} rows w  ere updated {text}");
                }
            }




            return new OkResult();
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
//        public static async Task<IActionResult> Run(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "PostFunction")] HttpRequest req,
//            ILogger log,
//            [Sql("dbo.measurements", ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<Measurement> measurements)
//        {
//            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//            Measurement measurement = JsonConvert.DeserializeObject<Measurement>(requestBody);



//            await measurements.AddAsync(measurement);
//            await measurements.FlushAsync();
//            List<Measurement> measurementList = new List<Measurement> { measurement};

//            return new OkObjectResult(measurementList);
//        }
//    }
//}

