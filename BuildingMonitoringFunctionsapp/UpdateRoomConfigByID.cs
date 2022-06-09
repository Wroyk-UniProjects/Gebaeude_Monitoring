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
using System.Text;

namespace BuildingMonitoringFunctionsapp
{

    public class UpdateRoomConfigByID {
        private readonly ILogger<UpdateRoomConfigByID> _logger;

        public UpdateRoomConfigByID(ILogger<UpdateRoomConfigByID> log)
        {
            _logger = log;
        }
        [FunctionName("updateRoomConfigByID")]

        public async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "rooms/{roomID}/roomConfig")] HttpRequest req, int roomID)
        {
            //  Read request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");

            //  Convert JSON Object in Measurement Object
            RoomConfig roomConfig = JsonConvert.DeserializeObject<RoomConfig>(requestBody);

            using (SqlConnection connection = new SqlConnection(connection_str))
            {

                //  SQL query
                var sql_query = "update roomConfig set " +
                    "  [target.temper] = " + roomConfig.targetTemper +
                    ", [targetHumid] = " + roomConfig.targetHumid +
                    ", [updateRate] = " + roomConfig.updateRate +
                    ", [upperToleranceTemper] =" + roomConfig.upperToleranceTemper +
                    ", [lowerToleranceTemper] =" + roomConfig.lowerToleranceTemper +
                    ", [upperToleranceHumid] =" + roomConfig.upperToleranceHumid +
                    ", [lowerToleranceHumid] =" + roomConfig.lowerToleranceHumid +
                    ", where id = @roomID";

                //  Create command
                SqlCommand sql_cmd = new SqlCommand(sql_query, connection);

                //  Create parameter from Route
                sql_cmd.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
                sql_cmd.Parameters[sql_cmd.Parameters.Count - 1].Value = roomID;

                StringBuilder errorMessages = new StringBuilder();

                //  Try to connect and execute query
                try
                {
                    connection.Open();
                    var rows = await sql_cmd.ExecuteNonQueryAsync();
                    connection.Close();
                }
                catch (SqlException ex)
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
