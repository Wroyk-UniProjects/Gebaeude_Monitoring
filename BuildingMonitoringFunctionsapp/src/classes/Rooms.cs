namespace BuildingMonitoringFunctionsapp
{
    //  Klasse fuer die Azure Funktion 'getRooms'
    public class Rooms
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool individual { get; set; }
        public string status { get; set; }
        public double hum{ get; set; }
        public double temp{ get; set; }
        public double targetTemp { get; set; }
        public double targetHum { get; set; }
    }
}

