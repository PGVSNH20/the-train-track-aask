using System;

namespace TrainEngine
{
    public class TrackOrm
    {
        public static TrackDescription ParseTrackDescription(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            var track = lines[0];

            TrackDescription trackDescription = new TrackDescription();
            char[] chars = track.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '[')
                {
                    var stations = TravelPlan.GetStations(@"Data\stations.txt");
                    var id = Convert.ToInt32(chars[i + 1]);
                    var station = stations.Find(st => st.StationID == id);
                    trackDescription.Parts.Add(station);
                }

                if (chars[i] == '-' || chars[i] == '/' || chars[i] == '\\')
                {
                    trackDescription.Parts.Add(new Length());
                }

                if (chars[i] == '=')
                {
                    trackDescription.Parts.Add(new Crossing());
                }

                if (chars[i] == '<' || chars[i] == '>')
                {
                    trackDescription.Parts.Add(new Switch());
                }
            }

            return trackDescription;
        }
    }
}