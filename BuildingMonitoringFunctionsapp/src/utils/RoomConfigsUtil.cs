﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BuildingMonitoringFunctionsapp.src.utils
{
    internal class RoomConfigsUtil
    {
        private static readonly string query = "select [targetTemper],[targetHumid],[updateRate],[upperToleranceTemper],[lowerToleranceTemper],[upperToleranceHumid]," +
                                      "[lowerToleranceHumid] from roomConfig";

        public static List<RoomConfig> getRoomConfigList()
        {
            List<RoomConfig> roomConfigsList = new List<RoomConfig>(); 

            var connection_str = Environment.GetEnvironmentVariable("sqldb_connection");

            using (SqlConnection connection = new SqlConnection(connection_str))
            {
                //  Create SQL command based on connection
                SqlCommand sql_cmd = new SqlCommand(query, connection);

                //sql_cmd.Parameters.Add("@roomId", System.Data.SqlDbType.Int);
                //sql_cmd.Parameters[sql_cmd.Parameters.Count - 1].Value = roomID;

                //  Used to show errors, if any
                StringBuilder errorMessages = new StringBuilder();

                //  Try to connect and execute query
                try
                {
                    connection.Open();
                    using (sql_cmd)
                    {
                        var roomconfig_var = sql_cmd.ExecuteReader();
                        roomConfigsList = Utils.getList<RoomConfig>(roomconfig_var);
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
            return roomConfigsList;
        }
    }
}
