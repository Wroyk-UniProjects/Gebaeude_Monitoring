using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace BuildingMonitoringFunctionsapp
{
    public static class GetRoomConfigByID
    {
        [FunctionName("getRoomConfigByID")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rooms/{ID}/config")] HttpRequest req, int ID)
        {
            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                try
                {
                    // return new OkObjectResult(new RoomConfig(ID, connection));
                    return new OkObjectResult(new RoomConfig());

                }
                catch (Exception ex)
                {
                    return new BadRequestObjectResult(ex.Message);
                }
            }
        }
    }
}
