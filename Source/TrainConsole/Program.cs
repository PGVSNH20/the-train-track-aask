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

            //var travelPlan = new TravelPlan(trains[2]).StartAt("Gävle", "12:00").ArriveAt("Uppsala", "13:05").GeneratePlan();
            //travelPlan.Save("minTravelPlan.json");

            //var travelPlan = TravelPlan.Load("minTravelPlan.json");


            Clock time = new Clock();
            Thread timeThread = new Thread(time.RunTime);
            timeThread.Start();
            var trains = TravelPlan.GetTrains(@"Data\trains.txt");
            var stations = TravelPlan.GetStations(@"Data\stations.txt");

            /* var td = TrackOrm.ParseTrackDescription(@"Data\traintrack1.txt");
             var travelPlan = new TravelPlan(trains[2], td)
                 .AddEntry(new TimeTableEntry() { TrainID = 3, DepartureTime = "12:00", ArrivalTime = null, Station = stations[0] })
                 .AddEntry(new TimeTableEntry() { TrainID = 3, DepartureTime = null, ArrivalTime = "12:15", Station = stations[2] });

             travelPlan.Simulate(time);
            */

            var td = TrackOrm.ParseTrackDescription("traintrack2.txt");
            var travelPlan = new TravelPlan(trains[3], td)
                .AddEntry(new TimeTableEntry() { TrainID = 4, DepartureTime = "12:00", ArrivalTime = null, Station = stations[0]})
                .AddEntry(new TimeTableEntry() { TrainID = 4, DepartureTime = "12:15", ArrivalTime = "12:12", Station = stations[1]})
                .AddEntry(new TimeTableEntry() { TrainID = 4, DepartureTime = null, ArrivalTime = "12:28", Station = stations[2]});
                
            travelPlan.Simulate(time);
        }
    }
}
