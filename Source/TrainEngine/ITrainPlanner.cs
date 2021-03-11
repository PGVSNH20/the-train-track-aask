using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public interface ITrainPlanner
    {
        public ITrainPlanner HeadTowards(Station station);
        public ITrainPlanner StartTrainAt(Station station, string departureTime);
        public ITrainPlanner StopTrainAt(Station station, string arrivalTime);
        public ITravelPlan GeneratePlan();



    }
}
