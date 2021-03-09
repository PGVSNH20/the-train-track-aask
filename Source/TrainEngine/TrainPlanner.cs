using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public class TrainPlanner : ITravelPlan
    {
        public Train Train { get; }
        public Station StartStation { get; }
        public Station EndStation { get; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public List<object> TimeTable => throw new NotImplementedException();

        public TrainPlanner(Train train, Station station)
        {
            Train = train;
            StartStation = station;
        }

        public ITravelPlan HeadTowards()
        {
            return this;
        }

        public ITravelPlan StartTrainAt()
        {
            return this;
        }

        public ITravelPlan StopTrainAt()
        {
            return this;
        }

        public ITravelPlan GeneratePlan()
        {
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
