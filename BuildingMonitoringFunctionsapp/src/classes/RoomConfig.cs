using System;
using System.Data.SqlClient;
using System.Text;

namespace BuildingMonitoringFunctionsapp
{
    //  Klasse fuer die Azure Funktion 'getRoomConfig'
    public class RoomConfig
    {
        private static string queryID = "select [roomId],[targetTemp],[targetHum],[updateRate],[uperToleranceT],[lowerToleranceT],[uperToleranceH],[lowerToleranceH] from roomConfig  " +
                                            "where [roomId] = @roomID";
        public int roomId { get; set; }

        public double targetTemp { get; set; }

        public double targetHum { get; set; }

        public double updateRate { get; set; }

        public double upperToleranceT { get; set; }

        public double lowerToleranceT { get; set; }

        public double upperToleranceH { get; set; }

        public double lowerToleranceH { get; set; }

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
                            this.targetTemp = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("targetTemp"));
                            this.targetHum = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("targetHum"));
                            this.updateRate = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("updateRate"));
                            this.upperToleranceT = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("uperToleranceT"));
                            this.lowerToleranceT = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("lowerToleranceT"));
                            this.upperToleranceH = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("uperToleranceH"));
                            this.lowerToleranceH = roomconfig_var.GetDouble(roomconfig_var.GetOrdinal("lowerToleranceH"));
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
            var sql_query = "update roomconfig set [targetTemp]=" + roomConfig.targetTemp + ", [targetHum]=" + roomConfig.targetHum + ", [updateRate]=" + roomConfig.targetTemp + ", [uperToleranceT]=" + roomConfig.targetTemp + "," +
                            "[uperToleranceH]=" + roomConfig.targetTemp + ",  [lowerToleranceT]=" + roomConfig.targetTemp + ", [lowerToleranceH]=" + roomConfig.targetTemp + ", " +
                            "[roomId]= " + roomConfig.targetTemp + " where [roomId] = " + roomConfig.roomId;

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

