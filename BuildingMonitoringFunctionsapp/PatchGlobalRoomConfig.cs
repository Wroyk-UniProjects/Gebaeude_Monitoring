using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Text;

namespace BuildingMonitoringFunctionsapp
{
    public class PatchGlobalRoomConfig
    {
    private readonly ILogger<PatchGlobalRoomConfig> _logger;

        public PatchGlobalRoomConfig(ILogger<PatchGlobalRoomConfig> log)
        {
            _logger = log;
        }

        [FunctionName("patchGlobalRoomConfig")]
        public async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "rooms/config")] HttpRequest req)
        {
            //  Read request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");

            //  Convert JSON Object in roomConfig Object
            RoomConfig roomConfig= JsonConvert.DeserializeObject<RoomConfig>(requestBody);
            
            using (SqlConnection connection = new SqlConnection(connection_str))
            {

                //  SQL query
                var sql_query =
                    " update roomConfig set" +
                    " [targetTemper] = " + roomConfig.targetTemper +", " +
                    " [targetHumid] = " + roomConfig.targetHumid + 
                    ", [updateRate] = " + roomConfig.updateRate +
                    ", [upperToleranceTemper] = " + roomConfig.upperToleranceTemper +
                    ", [lowerToleranceTemper] = " + roomConfig.lowerToleranceTemper +
                    ", [upperToleranceHumid] = " + roomConfig.lowerToleranceHumid +
                    ", [lowerToleranceHumid] = " + roomConfig.lowerToleranceHumid + "where id = 0";

                //  Create command
                SqlCommand sql_cmd = new SqlCommand(sql_query, connection);

                StringBuilder errorMessages = new StringBuilder();

                //  Try to connect and execute query
                try
                {
                    connection.Open();
                    var rows = await sql_cmd.ExecuteNonQueryAsync();
                    connection.Close();
                }
                catch(SqlException ex)
                {
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append("Index #" + i + "\n" +
                            "Message: " + ex.Errors[i].Message + "\n" +
                            "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                            "Source: " + ex.Errors[i].Source + "\n" +
                            "Procedure: " + ex.Errors[i].Procedure + "\n");
                    }
                    _logger.LogInformation(ex.ToString());
                    return new BadRequestResult();
                }
            }
            return new OkResult();
        }
    }
}


