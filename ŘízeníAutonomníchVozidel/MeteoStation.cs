using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ŘízeníAutonomníchVozidel
{
    class MeteoStation
    {
        static Random RNG = new Random();
        //Vlastnosti
        public static string Weather { get; set; }
        public static string TimeOfDay { get; set; }
        public static int WindForce { get; set; }
        //Day_Night: true = Den ; false = Noc
        public static bool Day_Night { get; set; }
        public static bool Rain { get; set; }
        public static bool Snow { get; set; }

        static Timer dnTimer;
        static Timer wTimer;
        public static void StartWeather()
        {
            //DefaultSet
            Day_Night = true;
            TimeOfDay = "Day";
            Weather = "Normal";
            Rain = false;
            Snow = false;
            WindForce = 3;
            //DayNightCycle
            dnTimer = new Timer(120000);
            dnTimer.Elapsed += Day_NightSwitch;
            dnTimer.AutoReset = true;
            dnTimer.Enabled = true;
            //RandomWeatherGenerator
            wTimer = new Timer(RNG.Next(40000,120000));
            wTimer.Elapsed += WeatherChange;
            wTimer.AutoReset = true;
            wTimer.Enabled = true;
        }
        public static void Day_NightSwitch(Object sender, ElapsedEventArgs e)
        {
            if (Day_Night)
            {
                Day_Night = false;
                TimeOfDay = "Night";
            }
            else
            {
                Day_Night = true;
                TimeOfDay = "Day";
            }
        }
        public static void WeatherChange(Object sender, ElapsedEventArgs e)
        {
            WindForce = RNG.Next(11);
            switch (RNG.Next(3))
            {
                //Normal
                case 0:
                    Weather = "Normal";
                    Rain = false;
                    Snow = false;
                    break;
                //Rain
                case 1:
                    Weather = "Raining";
                    Rain = true;
                    Snow = false;
                    break;
                //Snow
                case 2:
                    Weather = "Snowing";
                    Rain = false;
                    Snow = true;
                    break;
            }
            wTimer.Interval = RNG.Next(40000, 120000);
        }
    }
}
