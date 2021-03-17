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

            Clock time = new Clock();
            Thread timeThread = new Thread(time.RunTime);
            timeThread.Start();
            var trains = TrainOrm.GetTrains(@"Data\trains.txt");
            var stations = StationsOrm.GetStations(@"Data\stations.txt");

            var td = TrackOrm.ParseTrackDescription(@"Data\traintrack1.txt");
             var travelPlan = new TravelPlan(trains[2], td)
                 .AddEntry(new TimeTableEntry() { TrainID = 3, DepartureTime = "12:00", ArrivalTime = null, Station = stations[0] })
                 .AddEntry(new TimeTableEntry() { TrainID = 3, DepartureTime = null, ArrivalTime = "12:15", Station = stations[2] });

             travelPlan.Simulate(time);

            /*
            var td = TrackOrm.ParseTrackDescription("traintrack2.txt");
            var signals = new Signals(time, td);
            Thread signalsThread = new Thread(signals.Simulate);
            signalsThread.Start();

            var travelPlan = new TravelPlan(trains[1], td)
                .AddEntry(new TimeTableEntry() { TrainID = 2, DepartureTime = "12:00", ArrivalTime = null, Station = stations[0]})
                .AddEntry(new TimeTableEntry() { TrainID = 2, DepartureTime = "12:17", ArrivalTime = "12:12", Station = stations[1]})
                .AddEntry(new TimeTableEntry() { TrainID = 2, DepartureTime = null, ArrivalTime = "12:30", Station = stations[2]});

            travelPlan.Simulate(time);            
            
            var travelPlanTwo = new TravelPlan(trains[2], td)
                .AddEntry(new TimeTableEntry() { TrainID = 3, DepartureTime = "12:05", ArrivalTime = null, Station = stations[0]})
                .AddEntry(new TimeTableEntry() { TrainID = 3, DepartureTime = "12:20", ArrivalTime = "12:17", Station = stations[1]})
                .AddEntry(new TimeTableEntry() { TrainID = 3, DepartureTime = null, ArrivalTime = "12:33", Station = stations[2]});

            travelPlanTwo.Simulate(time); 
             */
        }
    }
}
