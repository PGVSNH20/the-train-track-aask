using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public class TrainPlanner
    {
        Train Train { get; }
        Station StartStation { get; }
        Station EndStation { get; }
        string DepartureTime { get; set; }
        string ArrivalTime { get; set; }

        public TrainPlanner(Train train, Station station)
        {
            Train = train;
            StartStation = station;
        }


    }
}
