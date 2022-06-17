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

        public static List<Measurement> getMeasurementsList()
        {
            List<Measurement> measurements = new List<Measurement>();
            var connection_str = Environment.GetEnvironmentVariable("sqlconnectionstring");

            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                //  Create SQL command based on connection
                SqlCommand sql_cmd = new SqlCommand(query, connection);
                //  Try to connect and execute query
                try
                {
                    connection.Open();
                    using (sql_cmd)
                    {
                        using (var roomconfig_var = sql_cmd.ExecuteReader())
                        {
                            while (roomconfig_var.Read())
                            {
                                Measurement measurement = new Measurement();

                                measurement.roomId = roomconfig_var.GetInt16(roomconfig_var.GetOrdinal("roomId"));
                                measurement.humid = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("temper"));
                                measurement.temper = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("humid"));
                                measurement.date = roomconfig_var.GetDateTime(roomconfig_var.GetOrdinal("date"));

                                measurements.Add(measurement);
                            }
                        }
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {

                }
            }
            return measurements;
        }


        public static Measurement createMeasurement(int roomID, SqlConnection connection)
        {
            Measurement measurement = new Measurement();
            //  Create SQL command based on connection
            SqlCommand sql_cmd = new SqlCommand(query, connection);

            sql_cmd.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
            sql_cmd.Parameters[sql_cmd.Parameters.Count - 1].Value = roomID;

            //  Try to connect and execute query
            try
            {
                connection.Open();
                using (sql_cmd)
                {
                    using (var measurement_var = sql_cmd.ExecuteReader())
                    {
                        while (measurement_var.Read())
                        {
                            measurement.roomId = measurement_var.GetInt32(measurement_var.GetOrdinal("roomId"));
                            measurement.humid = measurement_var.GetDouble(measurement_var.GetOrdinal("temper"));
                            measurement.temper = measurement_var.GetDouble(measurement_var.GetOrdinal("humid"));
                            measurement.date = measurement_var.GetDateTime(measurement_var.GetOrdinal("date"));
                        }
                    }
                }
                connection.Close();
                return measurement;
            }
            catch (SqlException ex)
            {
                return null;
            }
        }



    }
}
