namespace Building_Monitoring_WebApp.Model
{
    public class RoomDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
        public string Status { get; set; }
        public double Humid { get; set; }
        public double Temper { get; set; }
        public double TargetHumid { get; set; }
        public double TargetTemper { get; set; }
        public String ImageUrl { get; set; }
    }
}
