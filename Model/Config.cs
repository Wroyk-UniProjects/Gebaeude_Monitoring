namespace Building_Monitoring_WebApp.Model
{
    public class Config
    {
        public int Id { get; set; }
        public double TargetTemper { get; set; }
        public double TargetHumid { get; set; }
        public double UpdateRate { get; set; }
        public double UpperToleranceTemper { get; set; }
        public double LowerToleranceTemper { get; set; }
        public double UpperToleranceHumid { get; set; }
        public double LowerToleranceHumid { get; set; }

        public Config()
        {

        }

        public Config(Config cnfgToCopy)
        {
            Id = cnfgToCopy.Id;
            TargetTemper = cnfgToCopy.TargetTemper;
            TargetHumid = cnfgToCopy.TargetHumid;
            UpdateRate = cnfgToCopy.UpdateRate;
            UpperToleranceTemper = cnfgToCopy.UpperToleranceTemper;
            LowerToleranceTemper = cnfgToCopy.LowerToleranceTemper;
            UpperToleranceHumid = cnfgToCopy.UpperToleranceHumid;
            LowerToleranceHumid = cnfgToCopy.LowerToleranceHumid;
        }
    }
}
