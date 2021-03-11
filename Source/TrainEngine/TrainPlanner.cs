using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public class TrainPlanner : ITrainPlanner
    {
        public Train Train { get; }
        public Station StartStation { get; }
        public Station EndStation { get; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public List<TimeTableEntry> TimeTable { get; set; }

        public TrainPlanner(Train train, Station station)
        {
            Train = train;
            StartStation = station;
        }

        public ITrainPlanner HeadTowards(Station station)
        {
            return this;
        }

        public ITrainPlanner StartTrainAt(string departureTime)
        {
            return this;
        }

        public ITrainPlanner StopTrainAt(Station station, string arrivalTime)
        {
            return this;
        }

        public ITravelPlan GeneratePlan()
        {
            //skapa travelPlan objekt
            TravelPlan travelplan = new TravelPlan(Train);
            return this;
        }

        public void Save(string path)
        {
            throw new NotImplementedException();
        }

        public void Load(string path)
        {
            throw new NotImplementedException();
        }
    }
}
