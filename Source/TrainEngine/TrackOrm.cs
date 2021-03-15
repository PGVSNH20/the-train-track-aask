using System;

namespace TrainEngine
{
    public class TrackOrm
    {
        public TrackDescription ParseTrackDescription(string track)
        {
            TrackDescription trackDescription = new TrackDescription();
            char[] chars = track.ToCharArray();
            for(int i = 0; i < chars.Length; i++)
            {
                if(chars[i] == '[')
                {
                    var stations = TravelPlan.GetStations(@"Data\stations.txt");
                    var id = Convert.ToInt32(chars[i + 1]);
                    var station =  stations.fi
                    trackDescription.Parts.Add(new Station());
                }
            }

        }
    }
}