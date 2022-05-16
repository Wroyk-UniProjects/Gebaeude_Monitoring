using Microsoft.OData.Edm;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BuildingMonitoringFunctionsapp
{
    public class Measurement
    {
        public int roomId { get; set; }
        public double hum { get; set; }
        public double temper { get; set; }

    }

}
