namespace Building_Monitoring_WebApp.Model
{
    public class Config
    {
        public int Id { get; set; }
        public double TargetHumi { get; set; }
        public double UpToleHumi { get; set; }
        public double LoToleHumi { get; set; }
        public double TargetTemp { get; set; }
        public double UpToleTemp { get; set; }
        public double LoToleTemp { get; set; }
        public double UpdateRate { get; set; }
    }
}
