using System;
using System.Data.SqlClient;
using System.Text;

namespace BuildingMonitoringFunctionsapp
{
    //  Klasse fuer die Azure Funktion 'getRoomConfig'
    public class RoomConfig
    {
        private static string queryID = "select [targetTemper],[targetHumid],[updateRate],[upperToleranceTemper],[lowerToleranceTemper],[upperToleranceHumid],[lowerToleranceHumid] from roomConfig  " +
                                            "where [id] = @roomID";
        public int roomId { get; set; }

        public double targetTemper { get; set; }

        public double targetHumid { get; set; }

        public double updateRate { get; set; }

        public double upperToleranceTemper { get; set; }

        public double lowerToleranceTemper { get; set; }

        public double upperToleranceHumid { get; set; }

        public double lowerToleranceHumid { get; set; }

        public RoomConfig(int roomID, SqlConnection connection)
        {
            //  Create SQL command based on connection
            SqlCommand sql_cmd = new SqlCommand(queryID, connection);

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
                            this.roomId = roomconfig_var.GetInt32(roomconfig_var.GetOrdinal("roomId"));
                            this.targetTemper = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("targetTemper"));
                            this.targetHumid = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("targetHumid"));
                            this.updateRate = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("updateRate"));
                            this.upperToleranceTemper = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("uperToleranceTemper"));
                            this.lowerToleranceTemper = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("lowerToleranceTemper"));
                            this.upperToleranceHumid = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("uperToleranceHumid"));
                            this.lowerToleranceHumid = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("lowerToleranceHumid"));
                        }
                    }
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
            }
        }

        public static void updateRoomConfig(RoomConfig roomConfig, SqlConnection connection)
        {
            var sql_query = "update roomconfig set [targetTemper]=" + roomConfig.targetTemper + ", [targetHumid]=" + roomConfig.targetHumid + ", [updateRate]=" + roomConfig.targetTemper + ", [uperToleranceTemper]=" + roomConfig.targetTemper + "," +
                            "[uperToleranceHumid]=" + roomConfig.targetTemper + ",  [lowerToleranceTemper]=" + roomConfig.targetTemper + ", [lowerToleranceHumid]=" + roomConfig.targetTemper + ", " +
                            "[roomId]= " + roomConfig.targetTemper + " where [roomId] = " + roomConfig.roomId;

            //  Create command
            SqlCommand sql_cmd = new SqlCommand(sql_query, connection);

            StringBuilder errorMessages = new StringBuilder();

            //  Try to connect and update room config
            try
            {
                connection.Open();
                using (sql_cmd)
                {
                    sql_cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}



// int ID, double targetTemp, double targetHum, double updateRate, double upperToleranceT,double lowerToleranceT, double upperToleranceH, double lowerToleranceH, SqlConnection connection

