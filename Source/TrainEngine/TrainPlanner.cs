using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    //vårt ORM?
    public class TrainPlanner : ITrainPlanner
    {
        public Train Train { get; }

        private Station StartStation { get; }
        private Station EndStation { get; set; }
        private string DepartureTime { get; set; }
        private string ArrivalTime { get; set; }

        //public List<TimeTableEntry> TimeTable { get; set; }

        public TrainPlanner(Train train, Station station)
        {
            Train = train;
            StartStation = station;
        }

        public ITrainPlanner HeadTowards(Station station)
        {
            EndStation = station;
            return this;
        }

        public ITrainPlanner StartTrainAt(Station station, string departureTime)
        {
            
            return this;
        }

        public ITrainPlanner StopTrainAt(Station station, string arrivalTime)
        {
            return this;
        }

        public ITravelPlan GeneratePlan()
        {
        //    //skapa travelPlan objekt
        //    //TravelPlan travelplan = new TravelPlan(Train);
            return null;
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
