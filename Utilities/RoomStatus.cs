﻿namespace Building_Monitoring_WebApp.Utilities
{
    public class RoomStatus
    {
        public enum States
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

        public static String getRoomStateCSS(String stateName)
        {
            switch (getRoomState(stateName))
            {
                case States.ok:
                    return "roomStatusOk";
                    break;

                case States.tooHigh:
                case States.tooHighTemp:
                case States.tooHighHum:
                    return "roomStatusTooHigh";
                    break;

                case States.tooLow:
                case States.tooLowTemp:
                case States.tooLowHum:
                    return "roomStatusTooLow";
                    break;

                case States.tooHighTemptooLowHum:
                    return "roomStatusHighTempLowHum";
                    break;

                case States.tooLowTemptooHighHum:
                    return "roomStatusLowTempHighHum";
                    break;

                case States.undefined:
                default:
                    return "roomStatusUndefined";
                    break;
            }
        }

        public static String getSimpelRoomStateText(String stateName)
        {
            return "";
        }

        public static States getRoomState(String stateName)
        {
            if (String.IsNullOrEmpty(stateName) || !Enum.GetNames(typeof(States)).Contains("stateName"))
            {
                // Wenn stateName nicht enhält oder ein Wert enthelt der nicht im enum ist wird undefined zurückgegeben
                return States.undefined;
            }
            else
            {
                // Den Status String zu States enum convertiren
                return (States)Enum.Parse(typeof(States), stateName);
            }
    }
    }
}
