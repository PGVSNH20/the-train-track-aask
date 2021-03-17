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

            Station endStation = (Station)TrainTrack.Parts.FindLast(part =>
            {
                return (part is Station station && station.EndStation);
            });

            while (SimulationIsRunning)
            {
                TimeTableEntry currentEntry = null;

                //Hitta den aktiva tidtabbels entry och tilldelar currentEntry
                for (int i = 0; i < TimeTable.Count; i++)
                {
                    if (!TimeTable[i].HasPassed)
                    {
                        currentEntry = TimeTable[i];
                        break;
                    }
                }

                //Kolla om det finns en aktiv entry, om ja körs följade kod
                if (currentEntry != null)
                {
                    
                }
                else
                {
                    //Om alla tidtabels entries är avklarade, avslutas simuleringen
                    SimulationIsRunning = false;
                }
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