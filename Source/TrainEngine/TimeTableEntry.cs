using System;

namespace TrainEngine
{
    public class TimeTableEntry
    {
        public int TrainID { get; set; }
        public Station Station { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }

        public bool HasPassed { get; set; } = false;
    }
}
