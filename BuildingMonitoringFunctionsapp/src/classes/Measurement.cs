using Microsoft.OData.Edm;

namespace BuildingMonitoringFunctionsapp
{
    public class Measurement
    {
        public int roomId { get; set; }
        public double hum { get; set; }
        public Date date { get; set; }
        public double temper { get; set; }
    }
}
