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
        noUpdateReceived,
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
        private static int timeoutScale = 3;
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

            Status tempStatus = getTempStatus(measurement, roomConfig, status);
            Status humidStatus = getHumidStatus(measurement, roomConfig, status);


            //Beide gleich
            if(tempStatus == Status.ok && humidStatus == Status.ok)
            {
                status = Status.ok;
            }
            if (tempStatus == Status.tooHighTemp && humidStatus == Status.tooHighHum)
            {
                tempStatus = Status.tooHigh;
            }
            if (humidStatus == Status.tooLowTemp && humidStatus == Status.tooLowHum)
            {
                humidStatus = Status.tooLow;
            }

            //Temberatur
            if (tempStatus == Status.tooHighTemp && humidStatus == Status.ok)
            {
                status = Status.tooHighTemp;
            }
            if (tempStatus == Status.tooLowTemp && humidStatus == Status.ok)
            {
                status = Status.tooLowTemp;
            }

            //Luftfeuchtigkeit
            if (tempStatus == Status.ok && humidStatus == Status.tooHighHum)
            {
                status = Status.tooHighHum;
            }
            if(tempStatus == Status.ok && humidStatus == Status.tooLowHum)
            {
                status=Status.tooLowHum;
            }

            //Gemister Status
            if (tempStatus == Status.tooHighTemp && humidStatus == Status.tooLowHum)
            {
                status = Status.tooHighTemptooLowHum;
            }
            if (tempStatus == Status.tooLowTemp && humidStatus == Status.tooHighHum)
            {
                status = Status.tooLowTemptooHighHum;
            }

            // Rasberry not sending updats
            DateTime now = DateTime.UtcNow;
            TimeSpan differenz = now - measurement.date;
            TimeSpan timeout = TimeSpan.FromSeconds(roomConfig.updateRate * timeoutScale);
            if (differenz > timeout)
            {
                status = Status.noUpdateReceived;
            }

            return status.ToString();
        }

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

            //Es werden nur Statusänderungen erkant.
            //Ist keine Statusänderung aufgetreten wird currentStatus bei behalten

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

            //Es werden nur Statusänderungen erkant.
            //Ist keine Statusänderung aufgetreten wird currentStatus bei behalten

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
