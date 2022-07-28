namespace BuildingMonitoringFunctionsapp
{
    //  Klasse fuer die Azure Funktion 'getRoomByID'
    public class RoomDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool global { get; set; }
        public string status { get; set; }
        public double humid { get; set; }
        public double temper { get; set; }
        public double targetTemper { get; set; }
        public double targetHumid { get; set; }
        public string imageUrl { get; set; }
    }
}

