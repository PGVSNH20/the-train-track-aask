using System;

namespace TrainEngine
{
    public class TimeTableEntry
    {
        public int TrainID { get; set; }
        public int StationID { get; set; }
        public DateTime ? DepartureTime { get; set; }
        public DateTime ? ArrivalTime { get; set; }
    }
}
