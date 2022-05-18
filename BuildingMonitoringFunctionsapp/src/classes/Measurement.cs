using System;

namespace BuildingMonitoringFunctionsapp
{
    public class Measurement
    {
        public int roomId { get; set; }
        public double hum { get; set; }
        public double temper { get; set; }
        public DateTime date { get; set; }
    }
}
