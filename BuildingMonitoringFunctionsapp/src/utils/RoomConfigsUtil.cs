using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BuildingMonitoringFunctionsapp.src.utils
{
    public class RoomConfigsUtil
    {
        private static string query = "select [id],[targetTemper],[targetHumid],[updateRate],[upperToleranceTemper],[lowerToleranceTemper],[upperToleranceHumid]," +
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




        public static List<RoomConfig> getRoomConfigs()
        {
            List<RoomConfig> rcs = new List<RoomConfig>();
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
                                RoomConfig rc = new RoomConfig();

                                rc.id = roomconfig_var.GetInt16(roomconfig_var.GetOrdinal("id"));
                                rc.targetTemper = roomconfig_var.GetFloat(roomconfig_var.GetOrdinal("targetTemper"));
                                rc.targetHumid = roomconfig_var.GetFloat(roomconfig_var.GetOrdinal("targetHumid"));
                                rc.updateRate = roomconfig_var.GetFloat(roomconfig_var.GetOrdinal("updateRate"));
                                rc.upperToleranceTemper = roomconfig_var.GetFloat(roomconfig_var.GetOrdinal("upperToleranceTemper"));
                                rc.lowerToleranceTemper = roomconfig_var.GetFloat(roomconfig_var.GetOrdinal("lowerToleranceTemper"));
                                rc.upperToleranceHumid = roomconfig_var.GetFloat(roomconfig_var.GetOrdinal("upperToleranceHumid"));
                                rc.lowerToleranceHumid = roomconfig_var.GetFloat(roomconfig_var.GetOrdinal("lowerToleranceHumid"));

                                rcs.Add(rc);
                            }
                        }
                    }
                    connection.Close();
                }
                catch (SqlException ex)
                {

                }
            }
            return rcs;
        }




        public static RoomConfig createRoomConfig(int roomID, SqlConnection connection)
        {
            RoomConfig rc = new RoomConfig();
            //  Create SQL command based on connection
            String query = "select[id],[targetTemper],[targetHumid],[updateRate],[upperToleranceTemper],[lowerToleranceTemper],[upperToleranceHumid]," +
                                      "[lowerToleranceHumid] from roomConfig where id=@roomID";
            SqlCommand sql_cmd = new SqlCommand(query, connection);

            sql_cmd.Parameters.Add("@roomID", System.Data.SqlDbType.Int);
            sql_cmd.Parameters[sql_cmd.Parameters.Count - 1].Value = roomID;

            //  Used to show errors, if any
            StringBuilder errorMessages = new StringBuilder();

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
                            rc.id = roomconfig_var.GetInt32(roomconfig_var.GetOrdinal("id"));
                            rc.targetTemper = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("targetTemper"));
                            rc.targetHumid = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("targetHumid"));
                            rc.updateRate = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("updateRate"));
                            rc.upperToleranceTemper = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("upperToleranceTemper"));
                            rc.lowerToleranceTemper = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("lowerToleranceTemper"));
                            rc.upperToleranceHumid = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("upperToleranceHumid"));
                            rc.lowerToleranceHumid = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("lowerToleranceHumid"));
                        }
                    }
                }
                connection.Close();
                return rc;
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
































    }
}
