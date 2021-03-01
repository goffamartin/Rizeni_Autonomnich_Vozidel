using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ŘízeníAutonomníchVozidel
{
    class ControlCentre
    {
        public static List<AVehicle> VehicleList = new List<AVehicle>();

        public static Timer GeneralTime = new Timer(5000);

        public static void ConditionsChanged(AVehicle thisVehicle, traceTypes nextTraceType)
        {
            //Světla ve dne a v noci
            if (MeteoStation.Day_Night)
                thisVehicle.BrighterLightsON = false;
            else
                thisVehicle.BrighterLightsON = true;
            //Stěrače v dešti
            if (MeteoStation.Rain)
                thisVehicle.Wipers = true;
            else
                thisVehicle.Wipers = false;

            thisVehicle.ActualSpeed = thisVehicle.TravelSpeed;
            //Změna typu cesty
            if (nextTraceType == traceTypes.Road)
            {
                if (MeteoStation.Snow || MeteoStation.Rain)
                {
                    thisVehicle.ActualSpeed -= 20;
                }
            }
            if (nextTraceType == traceTypes.Tunnel && thisVehicle.ActualSpeed >= 80)
            {
                thisVehicle.ActualSpeed -= 30;
                thisVehicle.BrighterLightsON = true;
                thisVehicle.Wipers = false;
            }
            if (nextTraceType == traceTypes.Bridge && thisVehicle.ActualSpeed >= 70)
            {
                //Snížení rychlosti na mostě společně v závislosti na síle větru(1-10)
                thisVehicle.ActualSpeed -= (20 + MeteoStation.WindForce);
                if (MeteoStation.Snow || MeteoStation.Rain)
                {
                    thisVehicle.ActualSpeed -= 20;
                }
            }
            Program.ListOfVehicles();
        }
    }
}
