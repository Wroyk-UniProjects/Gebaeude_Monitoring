using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BuildingMonitoringFunctionsapp.src.utils
{
    internal class MeasurementUtil
    {
        private static readonly string query = "select [roomId],[temper],[humid],[date] from measurement";

        public static List<Measurement> getMeasurementList()
        {
            List<Measurement> measurements = new List<Measurement>();

            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");

            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                //  Create SQL command based on connection
                SqlCommand sql_cmd = new SqlCommand(query, connection);

                //  Used to show errors, if any
                StringBuilder errorMessages = new StringBuilder();

                //  Try to connect and execute query
                try
                {
                    connection.Open();
                    using (sql_cmd)
                    {
                        var measurement_var = sql_cmd.ExecuteReader();
                        measurements = Utils.getList<Measurement>(measurement_var);
                    }
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
                    Console.WriteLine(errorMessages.ToString());
                    return null;
                }
            }
            return measurements;
        }
    }
}
