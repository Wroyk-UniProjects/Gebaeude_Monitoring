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
using BuildingMonitoringFunctionsapp.src.utils;

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
                measurement.date = DateTime.UtcNow;
                //string sqlFormattedDate = measurement.date.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                //  SQL query
                //var sql_query = "update measurement set [temper] = " + measurement.temper + ", [humid] = " + measurement.humid + ", [date] = '" + sqlFormattedDate + "' where roomId = @roomID";
                String sql_query = "update measurement set [temper] = @temper , [humid] = @humid, [date] = @date where roomId = @roomID";
                String sql_getStatus = "select [global], [status] from room where id=@roomID";
                String sql_setStatus = "update room set [status]=@status where id=@roomID";
                //  Create command
                SqlCommand sql_cmd = new SqlCommand(sql_query, connection);
                SqlCommand cmd_getStatus = new SqlCommand(sql_getStatus, connection);
                SqlCommand cmd_setStatus = new SqlCommand(sql_setStatus, connection);

                //  Create parameter from Route
                sql_cmd.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
                sql_cmd.Parameters[0].Value = roomID;
                sql_cmd.Parameters.Add("@temper", System.Data.SqlDbType.Float);
                sql_cmd.Parameters[1].Value = measurement.temper;
                sql_cmd.Parameters.Add("@humid", System.Data.SqlDbType.Float);
                sql_cmd.Parameters[2].Value = measurement.humid;
                sql_cmd.Parameters.Add("@date", System.Data.SqlDbType.DateTime);
                sql_cmd.Parameters[3].Value = measurement.date;

                cmd_getStatus.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
                cmd_getStatus.Parameters[cmd_getStatus.Parameters.Count - 1].Value = roomID;

                cmd_setStatus.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
                cmd_setStatus.Parameters.Add("@status", System.Data.SqlDbType.VarChar);
                cmd_setStatus.Parameters[0].Value = roomID;
                

                StringBuilder errorMessages = new StringBuilder();

                //  Try to connect and execute query
                try
                {
                    connection.Open();
                    var result = await cmd_getStatus.ExecuteReaderAsync();
                    result.Read();
                    bool global = result.GetBoolean(result.GetOrdinal("global"));
                    String status = result.GetString(result.GetOrdinal("status"));
                    connection.Close();

                    RoomConfig rc = RoomConfigsUtil.createRoomConfig((global ? 0 : roomID), connection);

                    connection.Open();
                    cmd_setStatus.Parameters[1].Value = StatusUtil.GetStatus(measurement, rc, status);
                    var status_chnged = await cmd_setStatus.ExecuteNonQueryAsync();
                    connection.Close();

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
                    _logger.LogError(ex.ToString());
                    //return new BadRequestObjectResult(ex);
                    //return new BadRequestResult();
                }
            }
            return new OkResult();
        }
    }
}


