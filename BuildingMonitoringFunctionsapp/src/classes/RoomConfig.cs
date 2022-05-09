namespace BuildingMonitoringFunctionsapp
{
    //  
    public class RoomConfig
    {
        public int id { get; set; }
        public double targetTemp{ get; set; }
        public double targetHum{ get; set; }
        public double updateRate { get; set; }
        public double upperToleranceT { get; set; }
        public double lowerToleranceT { get; set; }

        public double upperToleranceH { get; set; }
        public double lowerToleranceH { get; set; }
        public int roomId { get; set; }

    }
}

