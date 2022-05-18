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
    public class PutMeasurement
    {
    private readonly ILogger<PutMeasurement> _logger;

        public PutMeasurement(ILogger<PutMeasurement> log)
        {
            _logger = log;
        }

        [FunctionName("putMeasurement")]
        public async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "rooms/{roomID}/measurement")] HttpRequest req, int roomID)
        {
            //  Read request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");

            //  Convert JSON Object in Measurement Object
            Measurement measurement = JsonConvert.DeserializeObject<Measurement>(requestBody);
            
            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                //  Get current date
                measurement.date = DateTime.Now;
                string sqlFormattedDate = measurement.date.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                //  SQL query
                var sql_query = "update measurements set [temp] = " + measurement.temper + ", [hum] = " + measurement.hum + ", [date] = '" + sqlFormattedDate + "' where roomId = @roomID";

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


