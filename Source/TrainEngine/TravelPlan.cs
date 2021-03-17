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
        private object CurrentPart = null;
        private int PartIndex = 0;

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
            CurrentPart = TrainTrack.Parts[0];
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
            SimulationIsRunning = true;
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
                int entryIndex = 0;

                //Hitta den aktiva tidtabbels entry och tilldelar currentEntry
                for (int i = 0; i < TimeTable.Count; i++)
                {
                    if (!TimeTable[i].HasPassed)
                    {
                        currentEntry = TimeTable[i];
                        entryIndex = i;
                        break;
                    }
                }

                //Kolla om det finns en aktiv entry, om ja körs följade kod
                if (currentEntry != null)
                {
                    Station deptStation = currentEntry.Station;
                    Station arrStation = (entryIndex + 1) < TimeTable.Count ? TimeTable[entryIndex + 1]?.Station : null;
                    string deptTime = currentEntry.DepartureTime;
                    int distanceDriven = Train.Moving ? Train.MaxSpeed * minutesPassed : 0;

                    //Kolla om deptTime är samma som klockan om ja börjar åka
                    if (deptTime != null && (int.Parse(deptTime.Split(":")[1]) == int.Parse(time.Time.Split(":")[1])))
                    {
                        Train.Moving = true;
                        Console.ForegroundColor = Train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine($"{Train.TrainName} departed from {deptStation.StationName}");
                        Console.ForegroundColor = ConsoleColor.White;

                    }

                    int distanceToDrive = 0;

                    CheckSignals();
                    if (Train.Moving)
                    {
                        PartIndex++;
                        CurrentPart = TrainTrack.Parts[PartIndex];
                    }

                    //Om det finns en arrivalStation körs följande 
                    if(arrStation != null)
                    {
                        //Räkna ut avståndet till nästa station
                        int lengthToNextStation = GetLenghtsBetweenStations(deptStation, arrStation);
                        distanceToDrive = lengthToNextStation * 10;

                        //kolla om tåget har kört så långt som avståndet är till nästa station
                        //aka kolla om tåget har kommit fram till stationen
                        if(distanceDriven >= distanceToDrive)
                        {
                            if(arrStation.StationID == endStation.StationID)
                                Console.WriteLine($"{Train.TrainName} reached it's final destination {endStation.StationName}");
                            else
                                Console.WriteLine($"{Train.TrainName} arrived at station {arrStation.StationName}");

                            Train.Moving = false;
                            minutesPassed = 0;
                            currentEntry.HasPassed = true;
                        }
                    }
                    else
                    {
                        if(distanceDriven >= distanceToDrive)
                        {
                            currentEntry.HasPassed = true;
                        }
                    }

                    minutesPassed++;
                }
                else
                {
                    //Om alla tidtabels entries är avklarade, avslutas simuleringen
                    SimulationIsRunning = false;
                }
                Thread.Sleep(1000);
            }
        }

        private void CheckSignals()
        {
            if(Train.Moving)
            {
                var nextPart = TrainTrack.Parts[PartIndex + 1];
                if(nextPart is Crossing crossing)
                {
                    if(!crossing.barriersDown)
                    {
                        Train.Moving = false;
                        Console.ForegroundColor = Train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine($"{Train.TrainName} is waiting for Carlos to close crossing barriers.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Train.Moving = true;
                        Console.ForegroundColor = Train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine($"{Train.TrainName} passed a closed crossing.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            else
            {
                var nextPart = PartIndex + 1 == TrainTrack.Parts.Count ? null : TrainTrack.Parts[PartIndex + 1];
                if(nextPart != null)
                {
                    if(nextPart is Crossing crossing)
                    {
                        if(crossing.barriersDown)
                        {
                            Train.Moving = true;
                            Console.ForegroundColor = Train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                            Console.WriteLine($"Crossing is closed, {Train.TrainName} is continuing it's journey.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Train.Moving = false;
                        }
                    } 
                }
            }
        }
        public int GetLenghtsBetweenStations(Station dept,Station arr)
        {
            int lenghts = 0;

            //hämta index för nuvarande depStation i TrainTrack parts listan och tilldela depStationIndex
            int depStationIndex = TrainTrack.Parts.FindIndex(station =>
            {
                if (station is Station st)
                    if (st.StationID == dept.StationID)
                        return true;
                return false;
            });

            //använd index på nuvarande deptStation för att börja räkna delar mellan deptStation och arrStation
            foreach(object obj in TrainTrack.Parts.Skip(depStationIndex).ToList())
            {
                if (!(obj is Station)) lenghts++;

                if (obj is Station station)
                    if (station.StationID == arr.StationID) break;
            }

            return lenghts;
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