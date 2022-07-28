using System;
using System.Data.SqlClient;
using System.Text;

namespace BuildingMonitoringFunctionsapp
{
    //  Klasse fuer die Azure Funktion 'getRoomConfig'
    public class RoomConfig
    {
        public int id { get; set; }

        public double targetTemper { get; set; }

        public double targetHumid { get; set; }

        public double updateRate { get; set; }

        public double upperToleranceTemper { get; set; }

        public double lowerToleranceTemper { get; set; }

        public double upperToleranceHumid { get; set; }

        public double lowerToleranceHumid { get; set; }
    }
}
