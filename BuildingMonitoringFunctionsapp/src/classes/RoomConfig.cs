using System;
using System.Data.SqlClient;
using System.Text;

namespace BuildingMonitoringFunctionsapp
{
    //  Klasse fuer die Azure Funktion 'getRoomConfig'
    public class RoomConfig
    {
        public int id { get; set; }

        public float targetTemper { get; set; }

        public float targetHumid { get; set; }

        public float updateRate { get; set; }

        public float upperToleranceTemper { get; set; }

        public float lowerToleranceTemper { get; set; }

        public float upperToleranceHumid { get; set; }

        public float lowerToleranceHumid { get; set; }
    }
}
