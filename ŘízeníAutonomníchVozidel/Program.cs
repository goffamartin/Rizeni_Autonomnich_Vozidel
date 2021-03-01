using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ŘízeníAutonomníchVozidel
{
    class Program
    {
        static Random RNG = new Random();
        static void Main(string[] args)
        {
            ControlCentre.GeneralTime.Start();

            MeteoStation.StartWeather();
            new AVehicle(1, 20, 230,ConsoleColor.Red);
            new AVehicle(2, 20, 50,ConsoleColor.Green);
            new AVehicle(3, 20, 120,ConsoleColor.Yellow);
            new AVehicle(4, 20, 180,ConsoleColor.Magenta);
            new AVehicle(5, 20, 150,ConsoleColor.Cyan);
            new AVehicle(6, 20, 90,ConsoleColor.Gray);
            Console.ReadLine();
        }
        public static void ListOfVehicles()
        {
            Console.Clear();
            Console.WriteLine($@"Welcome in program: Řízení autonomních vozidel
Info: update every {ControlCentre.GeneralTime.Interval/1000} s
Current count of vehicles on the road: {ControlCentre.VehicleList.Count}
Force of wind: {MeteoStation.WindForce}
Weather: {MeteoStation.Weather}
Time of day: {MeteoStation.TimeOfDay}");
            foreach (AVehicle Vehicle in ControlCentre.VehicleList)
            {
                Console.SetCursorPosition(0, 6 + 3 * Vehicle.OrderNumber);
                Console.WriteLine($@"ID: {Vehicle.OrderNumber}, v: {Vehicle.ActualSpeed} km/h" +
                     $" Dist: {Vehicle.DistanceTraveled:F2} km, Lights: {Vehicle.BrighterLightsON}, Wipers: {Vehicle.Wipers}");
                Console.SetCursorPosition((int)Vehicle.DistanceTraveled, Console.CursorTop);
                Console.ForegroundColor = Vehicle.VehicleColor;
                Console.WriteLine('▄');
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < Vehicle.Trace.Length; i++)
                {
                    char x;
                    if (Vehicle.Trace[i] == traceTypes.Road)
                        x = '─';
                    else if (Vehicle.Trace[i] == traceTypes.Bridge)
                        x = 'B';
                    else
                        x = 'T';
                    Console.Write(x);
                }
                if (Vehicle.DistanceTraveled == Vehicle.FullTraceDistance)
                {
                    Console.ForegroundColor = Vehicle.VehicleColor;
                    Console.Write(" Finished");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
