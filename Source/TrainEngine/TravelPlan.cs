using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public class TravelPlan : ITravelPlan
    {
        private List<string> TimeTable { get; set; }

        private Train Train { get; set; }

        public void Load(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(string path)
        {
            throw new NotImplementedException();
        }

        public TravelPlan()
        {
            //Train = train;

            TimeTable = new List<string>();
        }

        //public void AddEntry(Station station, string depTime, string arTime)
        //{
        //    TimeTable.Add(new TimeTableEntry() { TrainID = Train.TrainID, StationID = station.StationID, DepartureTime = depTime, ArrivalTime = arTime});
        //}

        public ITravelPlan StartAt(string station, string depTime)
        {
            TimeTable.Add(station);
            TimeTable.Add(depTime);
            return this;
        }

        public ITravelPlan ArriveAt(string station, string arrTime)
        {
            TimeTable.Add(station);
            TimeTable.Add(arrTime);
            return this;
        }

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
    }
}
