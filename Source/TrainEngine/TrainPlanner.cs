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

        public ITrainPlanner HeadTowards()
        {
            return this;
        }

        public ITrainPlanner StartTrainAt()
        {
            return this;
        }

        public ITrainPlanner StopTrainAt()
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
