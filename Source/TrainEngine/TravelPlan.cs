using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;

namespace TrainEngine
{
    public class TravelPlan : ITravelPlan
    {
        private List<TimeTableEntry> timeTable;

        private Train train;

        private TrackDescription trainTrack { get; set; }
        private bool simulationIsRunning = false;
        private object currentPart = null;
        private int partIndex = 0;
        private int minutesPassed = 0;
        private Clock time;
        private Station endStation;
        private TimeTableEntry currentEntry = null;
        private int entryIndex = 0;

        public TravelPlan(Train train, TrackDescription td)
        {
            this.train = train;
            trainTrack = td;
            currentPart = trainTrack.Parts[0];
            timeTable = new List<TimeTableEntry>();

            endStation = (Station)trainTrack.Parts.FindLast(part =>
            {
                return (part is Station station && station.EndStation);
            });
        }

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

      

        public ITravelPlan AddEntry(TimeTableEntry entry)
        {
            timeTable.Add(entry);
            return this;
        }
        public ITravelPlan GeneratePlan()
        {
            return this;
        }

       

        public void Simulate(Clock time)
        {
            Thread thread = new Thread(() => Run(time));
            simulationIsRunning = true;
            thread.Start();
        }

        private void Run(Clock time)
        {
            this.time = time;

            while (simulationIsRunning)
            {
                UpdateCurrentEntry();

                //Kolla om det finns en aktiv entry, om ja körs följade kod
                if (currentEntry != null)
                {
                    Station deptStation = currentEntry.Station;
                    Station arrStation = (entryIndex + 1) < timeTable.Count ? timeTable[entryIndex + 1]?.Station : null;
                    string deptTime = currentEntry.DepartureTime;
                    int distanceDriven = train.Moving ? train.MaxSpeed * minutesPassed : 0;

                    //Kolla om deptTime är samma som klockan om ja börjar åka
                    if (deptTime != null && (int.Parse(deptTime.Split(":")[1]) == int.Parse(time.Time.Split(":")[1])))
                    {
                        train.Moving = true;
                        Console.ForegroundColor = train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine($"{train.TrainName} departed from {deptStation.StationName}");
                    }


                    CheckSignals();
                    if (train.Moving)
                    {
                        partIndex++;
                        currentPart = trainTrack.Parts[partIndex];
                    }

                    UpdateDistance(distanceDriven, deptStation, arrStation);

                    if(train.Moving)
                        minutesPassed++;
                }
                else
                {
                    //Om alla tidtabels entries är avklarade, avslutas simuleringen
                    simulationIsRunning = false;
                }
                Thread.Sleep(1000);
            }
        }

        private void UpdateDistance(int distanceDriven, Station deptStation, Station arrStation)
        {
            int distanceToDrive = 0;

            //Om det finns en arrivalStation körs följande 
            if (arrStation != null)
            {
                //Räkna ut avståndet till nästa station
                int lengthToNextStation = GetLenghtsBetweenStations(deptStation, arrStation);
                distanceToDrive = lengthToNextStation * 10;

                //kolla om tåget har kört så långt som avståndet är till nästa station
                //aka kolla om tåget har kommit fram till stationen
                if (distanceDriven >= distanceToDrive)
                {
                    if (arrStation.StationID == endStation.StationID)
                    {
                        Console.ForegroundColor = train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine($"{train.TrainName} reached it's final destination {endStation.StationName}");
                      
                    }
                    else
                    {
                        Console.ForegroundColor = train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine($"{train.TrainName} arrived at station {arrStation.StationName}");
                        
                    }
                        train.Moving = false;
                        minutesPassed = 0;
                        currentEntry.HasPassed = true;
                    
                }
            }
            else
            {
                if (distanceDriven >= distanceToDrive)
                {
                    currentEntry.HasPassed = true;
                }
            }

        }
        private void UpdateCurrentEntry()
        {
            //Hitta den aktiva tidtabbels entry och tilldelar currentEntry
            for (int i = 0; i < timeTable.Count; i++)
            {
                if (!timeTable[i].HasPassed)
                {
                    currentEntry = timeTable[i];
                    entryIndex = i;
                    break;
                }
            }

        }

        private void CheckSignals()
        {
            if(train.Moving)
            {
                var nextPart = partIndex + 1 == trainTrack.Parts.Count ? null : trainTrack.Parts[partIndex + 1];
                if (nextPart != null) 
                {
                    if (nextPart is Crossing crossing)
                    {
                        if (!crossing.barriersDown)
                        {
                            train.Moving = false;
                            Console.ForegroundColor = train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                            Console.WriteLine($"{train.TrainName} is waiting for Carlos to close crossing barriers.");
                        }
                        else
                        {
                            train.Moving = true;
                            Console.ForegroundColor = train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                            Console.WriteLine($"{train.TrainName} passed a closed crossing.");
                        }
                    }
                }  
            }
            else
            {
                var nextPart = partIndex + 1 == trainTrack.Parts.Count ? null : trainTrack.Parts[partIndex + 1];
                if(nextPart != null)
                {
                    if(nextPart is Crossing crossing)
                    {
                        if(crossing.barriersDown)
                        {
                            train.Moving = true;
                            Console.ForegroundColor = train.TrainName == "Lapplandståget" ? ConsoleColor.Red : ConsoleColor.Green;
                            Console.WriteLine($"Crossing is closed, {train.TrainName} is continuing it's journey.");
                           
                        }
                        else
                        {
                            train.Moving = false;
                        }
                    } 
                }
            }
        }
        private int GetLenghtsBetweenStations(Station dept,Station arr)
        {
            int lenghts = 0;

            //hämta index för nuvarande depStation i TrainTrack parts listan och tilldela depStationIndex
            int depStationIndex = trainTrack.Parts.FindIndex(station =>
            {
                if (station is Station st)
                    if (st.StationID == dept.StationID)
                        return true;
                return false;
            });

            //använd index på nuvarande deptStation för att börja räkna delar mellan deptStation och arrStation
            foreach(object obj in trainTrack.Parts.Skip(depStationIndex).ToList())
            {
                if (!(obj is Station)) lenghts++;

                if (obj is Station station)
                    if (station.StationID == arr.StationID) break;
            }

            return lenghts;
        }

      
    }
}