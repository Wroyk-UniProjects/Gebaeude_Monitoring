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
using Nancy.Json;

namespace BuildingMonitoringFunctionsapp
{
    public class PostMeasurement
    {
        Measurement m ;



    private readonly ILogger<PostMeasurement> _logger;

        public PostMeasurement(ILogger<PostMeasurement> log)
        {
            _logger = log;
        }

        [FunctionName("postMeasurement")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]

        public async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rooms/measurement")] HttpRequest req)
        {
            string jsonString = JsonConvert.SerializeObject(m);
            Console.WriteLine(jsonString);
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            Measurement myClass = JsonConvert.DeserializeObject<Measurement>(content);



            _logger.LogInformation("hallo");

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
                  var text = "update roomConfig set [targetHum] = 500 where roomId= " + name + ";";
                //  var text = "insert into measurements ([roomId], [temp], [hum])  values( 2,55,  2)";
                // var text = "insert into measurements ([roomId], [temp], [hum])  values( 2,55,  2);";
                //var text = "update roomconfig set [targetTemp] = 100, [targetHum]= 1, [updateRate]= 1, [uperToleranceT]= 1," +
                //            "[uperToleranceH]=1,  [lowerToleranceT]=1, [lowerToleranceH]=1, " +
                //            "[roomId]= 2 where roomId= " + name + ";;

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


