using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ŘízeníAutonomníchVozidel
{
    public delegate void TraceChanded(AVehicle thisVehicle, traceTypes nextTraceType);
    public enum traceTypes { Road, Tunnel, Bridge };
    public class AVehicle
    {
        static Random RNG = new Random();

        //Vlastnosti
        public int OrderNumber { get; set; }
        public traceTypes[] Trace;
        public traceTypes CurrentTraceType { get; set; }
        public int FullTraceDistance { get; private set; }
        public double DistanceTraveled { get; set; }
        public double TravelSpeed { get; private set; }
        public double ActualSpeed { get; set; }
        public bool Wipers { get; set; }
        public bool BrighterLightsON { get; set; }
        public ConsoleColor VehicleColor { get; set; }
        
        public AVehicle(int orderNumber, int traceDistance, int speed, ConsoleColor color)
        {
            ControlCentre.VehicleList.Add(this);
            OrderNumber = orderNumber;
            FullTraceDistance = traceDistance;
            ActualSpeed = TravelSpeed = speed;
            VehicleColor = color;
            GenerateTrace();
            StartTrip();
        }
        //Random Generování trasy
        public void GenerateTrace()
        {
            Trace = new traceTypes[FullTraceDistance];
            for (int i = 0; i < FullTraceDistance; i++)
            {
                Trace[i] = (traceTypes)RNG.Next(3);
            }
        }

        public event TraceChanded traceChanged;
        // private static Timer timer;

        private void UpdateStatus(object sender, EventArgs args)
        {
            // Jeden nezávislý čas diktovaný ControlCentre
            // Ujetá vzdálenost vypočítaná z rychlosti a uplynulého času v km (proto "/3600")
            if(DistanceTraveled < FullTraceDistance)
            {
                DistanceTraveled += ActualSpeed * ControlCentre.GeneralTime.Interval / 1000.0 / 3600.0;
                if (DistanceTraveled > FullTraceDistance)
                {
                    //Když dojede do cíle tak se zastaví a vypne ostatní fukce;
                    DistanceTraveled = FullTraceDistance;
                    ActualSpeed = 0.0;
                    Wipers = false;
                    BrighterLightsON = false;
                }
                CurrentTraceType = Trace[(int)DistanceTraveled];
                traceChanged(this, CurrentTraceType);
            }
        }

        //Start jízdy
        public void StartTrip()
        {
            //nastavení defaultních hodnot
            DistanceTraveled = 0.0;
            CurrentTraceType = Trace[0]; 
            //Volání ConditionsChanged k přepočítání rychlosti na základě aktuálních podmínek 
            traceChanged += ControlCentre.ConditionsChanged;
            traceChanged(this, CurrentTraceType);
            //přiřazení methody UpdateStatus k timeru pro automatický update informací po určitém intervalu
            ControlCentre.GeneralTime.Elapsed += UpdateStatus;
            Program.ListOfVehicles();
        }
    }
}
