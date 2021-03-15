using System;
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

            Time time = new Time();
            Thread timeThread = new Thread(time.RunTime);
            timeThread.Start();

            var trains = TravelPlan.GetTrains(@"Data\trains.txt");
            var travelPlan = new TravelPlan(trains[2]).StartAt("Gävle", "12:00").ArriveAt("Uppsala", "13:05").GeneratePlan();

            travelPlan.Simulate(time);

        }
    }
}
