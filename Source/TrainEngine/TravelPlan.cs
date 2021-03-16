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

                int distancetoDrive = 0;
                int distanceDrived = 0;
                TimeTableEntry currentEntry = null;

                if(Train.Moving)
                {
                distanceDrived = Train.MaxSpeed * minutesPassed;
                }

                string depTime = "";
                
                var stations = TrainTrack.Parts.OfType<Station>().ToList();
                Station endStation = null;
                Station depStation = null;
                Station arrStation = null;
                

                for(int i = 0; i < TimeTable.Count; i++)
                {
                    if (TimeTable[i].Station.EndStation && !(TimeTable[0].Station.StationID == TimeTable[i].Station.StationID))
                    {
                        endStation = TimeTable[i].Station;
                    }

                    if (!TimeTable[i].HasPassed)
                    {
                        depStation = TimeTable[i].Station;
                        depTime = TimeTable[i].DepartureTime;
                        currentEntry = TimeTable[i];
                    }

                    //if not at last timetable entry set arrStation to next entry station
                    if (i != TimeTable.Count - 1 && distanceDrived >= distancetoDrive) arrStation = TimeTable[i + 1].Station;
                    //Console.WriteLine("Nu är arrStation: " + arrStation.StationName);

                    distancetoDrive = GetLengths(arrStation) * 10;

                }
                
                    if (depTime == time.Time)
                    {
                        Train.Moving = true;
                        Console.WriteLine("Train " + Train.TrainName + " departed from " + depStation.StationName + ": " + depTime);
                        currentEntry.HasPassed = true;
                    }

                    if (distanceDrived >= distancetoDrive)
                    {
                        Train.Moving = false;

                        Console.WriteLine("Train " + Train.TrainName + " arrived at " + arrStation.StationName);
                        Console.WriteLine("Train arrived after " + distanceDrived + " km");
                        Console.WriteLine($"Har åkt {distanceDrived} och ska åka {distancetoDrive}");

                        minutesPassed = 0;
                        distancetoDrive = 0;
                        distanceDrived = 0;

                        if (arrStation == endStation)
                        {
                            SimulationIsRunning = false;

                        }



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
                Thread.Sleep(500);
            }
        }

        private int GetLengths(Station station)
        {
            //calculates lengths between current stations
            int lengths = 0;
            for (int x = 0; x < TrainTrack.Parts.Count; x++)
            {
                //increments lengths if part is not a Station object
                if (TrainTrack.Parts[x].GetType() != typeof(Station))
                {
                    lengths++;
                }

                //breaks loop if part is arrival station
                if (TrainTrack.Parts[x] is Station)
                {
                    if ((TrainTrack.Parts[x] as Station).StationID == station.StationID) break;
                }
            }

            return lengths;
        }
        private string GetStationName(int id, List<Station> stations)
        {
           return stations.Find(st => st.StationID == id).StationName;
        }
    }
}