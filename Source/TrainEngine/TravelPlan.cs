using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading;

namespace TrainEngine
{
    public class TravelPlan : ITravelPlan
    {
        public List<TimeTableEntry> TimeTable { get; set; }

        private Train Train { get; set; }

        private TrackDescription TrainTrack { get; set; }
        private bool SimulationIsRunning = false;

        public static ITravelPlan Load(string path)
        {
            string travelPlanString = File.ReadAllText(path);
            TravelPlan tp = JsonSerializer.Deserialize<TravelPlan>(travelPlanString);
            return tp;
        }

        public void Save(string path)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            string travelPlanString = JsonSerializer.Serialize(this, options);
            File.WriteAllText(path, travelPlanString);
        }

        public TravelPlan(Train train, TrackDescription td)
        {
            Train = train;
            TrainTrack = td;
            TimeTable = new List<TimeTableEntry>();
        }

        public ITravelPlan AddEntry(TimeTableEntry entry)
        {
            TimeTable.Add(entry);
            return this;
        }

        /*public ITravelPlan StartAt(string station, string depTime)
        {
            TimeTable.Add(station + "," + depTime);
            return this;
        }

        public ITravelPlan ArriveAt(string station, string arrTime)
        {
            TimeTable.Add(station + "," + arrTime);
            return this;
        }*/

        public ITravelPlan GeneratePlan()
        {
            return this;
        }

        public static List<Station> GetStations(string fileName)
        {
            List<Station> stations = new List<Station>();
            string[] lines = System.IO.File.ReadAllLines(fileName);

            foreach(string line in lines)
            {
                string[] values = line.Split("|");
                if (values[0] == "Id") continue;
                var station = new Station(values[1]);
                station.StationID = Convert.ToInt32(values[0]);
                station.EndStation = Convert.ToBoolean(values[2]);
                stations.Add(station);
            }

            return stations;
        }

        public static List<Train> GetTrains(string fileName)
        {
            List<Train> trains = new List<Train>();
            string[] lines = System.IO.File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                string[] values = line.Split(",");
                if (values[0] == "Id") continue;
                var train = new Train(values[1]);
                train.TrainID = Convert.ToInt32(values[0]);
                train.MaxSpeed = Convert.ToInt32(values[2]);
                train.Operated = Convert.ToBoolean(values[3]);
                trains.Add(train);
            }

            return trains;
        }

        public void Simulate(Clock time)
        {
            Thread thread = new Thread(() => Run(time));
            thread.Start();
        }

        public void Run(Clock time)
        {
            int minutesPassed = 0;
            SimulationIsRunning = true;
            while (SimulationIsRunning)
            {

                var lengths = TrainTrack.Parts.OfType<Length>().ToList();
                int distancetoDrive = lengths.Count * 10;

                int distanceDrived = Train.MaxSpeed * minutesPassed;

                string depTime = TimeTable[0].DepartureTime;
                
                var stations = TrainTrack.Parts.OfType<Station>().ToList();
                Station endStation = null;
                

                foreach(var entry in TimeTable)
                {
                    if (entry.Station.EndStation)
                    {
                        endStation = entry.Station;
                    }

                }




                if (depTime == time.Time)
                {
                    Console.WriteLine("Train " + Train.TrainName + " departed from " + GetStationName(depStationID, stations) + ": " + depTime);
                }

                if (distanceDrived >= distancetoDrive)
                {
                    Console.WriteLine("Train " + Train.TrainName + " arrived at " + GetStationName(arrStationID, stations));
                    Console.WriteLine("Train arrived after " + distanceDrived + " km");
                    SimulationIsRunning = false;
                }

                //Console.WriteLine($"Har åkt {distanceDrived} och ska åka {distancetoDrive}");

                /*foreach(var entry in TimeTable)
                {
                    string departureTime = entry.DepartureTime;
                    string arrivalTime = entry.ArrivalTime;

                    if (departureTime == time.Time)
                    {
                        Console.WriteLine("Train " + Train.TrainName + " departed from " + entry.StationID + ": " + departureTime);
                    }


                    if (distanceDrived >= distancetoDrive)
                    {
                        Console.WriteLine("Train " + Train.TrainName + " arrival at " + entry.StationID + ": " + arrivalTime);
                        Console.WriteLine("Train arrived after " + distanceDrived + " km");
                    }
                }*/


                minutesPassed++;
                Thread.Sleep(1000);
            }
        }

        private string GetStationName(int id, List<Station> stations)
        {
           return stations.Find(st => st.StationID == id).StationName;
        }
    }
}