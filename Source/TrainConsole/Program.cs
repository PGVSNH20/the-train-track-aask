﻿using System;
using System.Threading;
using TrainEngine;

namespace TrainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Train track!");
            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)

            // Step 2:
            // Make the trains run in treads

            //var trains = TravelPlan.GetTrains(@"Data\trains.txt");
            //var travelPlan = new TravelPlan(trains[2]).StartAt("Gävle", "12:00").ArriveAt("Uppsala", "13:05").GeneratePlan();
            //travelPlan.Save("minTravelPlan.json");

            //var travelPlan = TravelPlan.Load("minTravelPlan.json");

            //var td = TrackOrm.ParseTrackDescription("traintrack2.txt");

            string time = "12:00";
            while (true)
            {
                string[] hourandminutes = time.Split(':');
                int hour = Convert.ToInt32(hourandminutes[0]);
                int minut = Convert.ToInt32(hourandminutes[1]);
                Thread.Sleep(200);
            }
            
        }
    }
}
