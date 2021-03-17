using System.Collections.Generic;

namespace TrainEngine
{
    public class TrackDescription
    {
        public int NumberOfTrackParts { get; set; }
        public List<object> Parts { get; private set; }

        public TrackDescription()
        {
            Parts = new List<object>();
        }
    }
}