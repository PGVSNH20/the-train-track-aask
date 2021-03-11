﻿using System;
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
            string[] lines = System.IO.File.ReadAllLines("station.txt");

            foreach(string line in lines)
            {
                string[] values = line.Split("|");
                var station = new Station(values[1]);
                station.StationID = Convert.ToInt32(values[0]);
                station.EndStation = Convert.ToBoolean(values[2]);
                stations.Add(station);
            }

            return stations;
            

        }

    }
}
