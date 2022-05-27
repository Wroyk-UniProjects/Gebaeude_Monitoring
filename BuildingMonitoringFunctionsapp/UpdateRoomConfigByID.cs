using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace BuildingMonitoringFunctionsapp
{
    public static class UpdateRoomConfigByID
    {
        [FunctionName("UpdateRoomConfigByID")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", Route = "rooms/{ID}/test")] HttpRequest req, int ID,
            ILogger log)
        {
            //  Read request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");

            RoomConfig new_roomConfig = JsonConvert.DeserializeObject<RoomConfig>(requestBody);

            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                try
                {
                    if (new_roomConfig == null)
                    {
                        try
                        {
                            return new OkObjectResult(new RoomConfig(ID, connection));
                        }
                        catch (Exception ex)
                        {
                            return new BadRequestObjectResult(ex);
                        }
                    }
                    else
                    {
                        RoomConfig.updateRoomConfig(new_roomConfig, connection);
                        return new OkResult();
                    }
                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex);
                }
            }
        }
    }
}
