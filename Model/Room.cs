namespace Building_Monitoring_WebApp.Model
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Individual { get; set; }
        public string Status { get; set; }
        public double Humid { get; set; }
        public double Temp { get; set; }
        public double TargetHum { get; set; }
        public double TargetTemp { get; set; }
    }
}
