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
            if (measurement.temper < (roomConfig.targetTemper + toleranceTemp))
            {
                status = Status.ok;
            }
            if (measurement.temper < (roomConfig.lowerToleranceTemper))
            {
                status = Status.tooLowTemp;
            }
            if (measurement.temper > (roomConfig.targetTemper - toleranceTemp))
        private static Status getHumidStatus(Measurement measurement, RoomConfig roomConfig, Status currentStatus)
        {
            Status status = currentStatus;

            switch (status)// den kombinirten status zu nur Lufeuchtigkeits status convertiren
            {
                case Status.tooHigh:
                case Status.tooLowTemptooHighHum:
                    status = Status.tooHighHum;
                    break;
                case Status.tooLow:
                case Status.tooHighTemptooLowHum:
                    status = Status.tooLowHum;
                    break;
                case Status.tooHighTemp:
                case Status.tooLowTemp:
                    status = Status.ok;
                    break;
            }

            double upperTolerance = (Math.Abs(roomConfig.upperToleranceHumid - roomConfig.targetHumid) / 2);
            double lowerTolerance = (Math.Abs(roomConfig.upperToleranceHumid - roomConfig.targetHumid) / 2);

            //Es werden nur Status�nderungen erkant.
            //Ist keine Status�nderung aufgetreten wird currentStatus bei behalten

            if (measurement.humid > (roomConfig.targetHumid - lowerTolerance))//muss vor tooHighHum kommen da im fall von tooHighHum auch true ist
            {
                status = Status.ok;
            }

            if (measurement.humid > roomConfig.upperToleranceHumid)
            {
                status = Status.tooHighHum;
            }

            if (measurement.humid < (roomConfig.targetHumid + upperTolerance))//muss vor tooLowHum kommen da im fall von tooLowHum auch true ist
            {
                status = Status.ok;
            }

            if (measurement.humid < (roomConfig.upperToleranceHumid))
            {
                status = Status.tooLowHum;
            }

            return status;
        }
        private static Status getTempStatus(Measurement measurement, RoomConfig roomConfig, Status currentStatus)
        {
            Status status = currentStatus;

            switch (status)// den kombinirten Status zu nur Temperatur Status convertiren
            {
                case Status.tooHigh:
                case Status.tooHighTemptooLowHum:
                    status = Status.tooHighTemp;
                    break;
                case Status.tooLow:
                case Status.tooLowTemptooHighHum:
                    status = Status.tooLowTemp;
                    break;
                case Status.tooHighHum:
                case Status.tooLowHum:
                    status = Status.ok;
                    break;
            }

            double upperTolerance = (Math.Abs(roomConfig.upperToleranceTemper - roomConfig.targetTemper) / 2);
            double lowerTolerance = (Math.Abs(roomConfig.lowerToleranceTemper - roomConfig.targetTemper) / 2);

            //Es werden nur Status�nderungen erkant.
            //Ist keine Status�nderung aufgetreten wird currentStatus bei behalten

            if (measurement.temper > (roomConfig.targetTemper - lowerTolerance))//muss vor tooHighTemp kommen da im fall von tooHighTemp auch true ist
            {
                status = Status.ok;
            }

            if (measurement.temper > roomConfig.upperToleranceTemper)
            {
                status = Status.tooHighTemp;
            }

            if (measurement.temper < (roomConfig.targetTemper + upperTolerance))//muss vor tooLowTemp kommen da im fall von tooLowTemp auch true ist
            {
                status = Status.ok;
            }

            if (measurement.temper < (roomConfig.lowerToleranceTemper))
            {
                status = Status.tooLowTemp;
            }

            return status;
        }
    }
}
