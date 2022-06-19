using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingMonitoringFunctionsapp.src.utils
{
    enum Status
    {
        undefined,
        ok,
        tooHigh,
        tooLow,
        tooHighTemp,
        tooLowTemp,
        tooHighHum,
        tooLowHum,
        tooHighTemptooLowHum,
        tooLowTemptooHighHum
    }
    internal class StatusUtil
    {
        public static string GetStatus(Measurement measurement, RoomConfig roomConfig, string currentStatus)
        {
            Status status;
            if (String.IsNullOrEmpty(currentStatus) || !Enum.GetNames(typeof(Status)).Contains(currentStatus))
            {
                status = Status.undefined;
            }
            else
            {
                status = (Status)Enum.Parse(typeof(Status), currentStatus);
            }

            double toleranceTemp = (Math.Abs(roomConfig.upperToleranceTemper-roomConfig.targetTemper) / 2);
            double toleranceHum = (Math.Abs(roomConfig.upperToleranceHumid - roomConfig.targetHumid) / 2);

            if (measurement.temper > roomConfig.upperToleranceTemper)
            {
                status = Status.tooHighTemp;
            }
            else if (measurement.temper < (roomConfig.targetTemper + toleranceTemp))
            {
                status = Status.ok;
            }
            else if (measurement.temper < (roomConfig.lowerToleranceTemper))
            {
                status = Status.tooLowTemp;
            }
            else if (measurement.temper > (roomConfig.targetTemper - toleranceTemp))
            {
                status = Status.ok;
            }


            if (measurement.humid > roomConfig.upperToleranceHumid && status.Equals(Status.tooHighTemp))
            {
                status = Status.tooHigh;
            }
            else if (measurement.humid < (roomConfig.targetHumid + toleranceHum) && status.Equals(Status.ok))
            {
                status = Status.ok;
            }
            else if (measurement.humid < roomConfig.lowerToleranceHumid && status.Equals(Status.tooLow))
            {
                status = Status.tooLow;
            }
            else if (measurement.humid > (roomConfig.targetHumid + toleranceHum) && status.Equals(Status.ok))
            {
                status = Status.ok;
            }
            else if ((measurement.temper > roomConfig.upperToleranceTemper) && (measurement.humid < roomConfig.lowerToleranceHumid))
            {
                status = Status.tooHighTemptooLowHum;
            }
            else if ((measurement.temper < (roomConfig.lowerToleranceTemper)) && (measurement.humid > roomConfig.upperToleranceHumid))
            {
                status = Status.tooLowTemptooHighHum;
            }

            return status.ToString();
        }
    }
}
