using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public class TravelPlan : ITravelPlan
    {
        private List<TimeTableEntry> TimeTable { get; set; }

        public Train Train { get; set; }

        public void Load(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(string path)
        {
            throw new NotImplementedException();
        }

        public TravelPlan(Train train)
        {
            Train = train;

            TimeTable = new List<TimeTableEntry>();
        }

        public void AddEntry(Station station, string depTime, string arTime)
        {
            TimeTable.Add(new TimeTableEntry() { TrainID = Train.TrainID, StationID = station.StationID, DepartureTime = depTime, ArrivalTime = arTime});
        }
    }
}
