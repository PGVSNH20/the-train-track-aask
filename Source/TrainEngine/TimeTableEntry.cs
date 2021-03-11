using System;

namespace TrainEngine
{
    public class TimeTableEntry
    {
        public int TrainID { get; set; }
        public int StationID { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
    }
}
