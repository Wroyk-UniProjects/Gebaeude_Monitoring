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


namespace BuildingMonitoringFunctionsapp
{
    public class UpdateMeasurementByRoomId
    {

        [FunctionName("updateMeasurementByRoomId")]
 
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req)
        {
            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            if (string.IsNullOrEmpty(name))
            {
                return new NotFoundResult();
            }
            else
            {
                // Get the connection string from app settings and use it to create a connection.
                var str = Environment.GetEnvironmentVariable("sqlconnectionstring");
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();
                    // var text = "update roomConfig set [targetTemp]="+name+" where id=1;";
                    var text = "update measurements set [temp]=2 ;";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        var rows = await cmd.ExecuteNonQueryAsync();
                    }
                }
                return new OkResult();
            }            
        }
    }
}

