using System;
using System.Collections.Generic;

namespace TrainEngine
{
    public class StationsOrm
    {
        public static List<Station> GetStations(string fileName)
        {
            List<Station> stations = new List<Station>();
            string[] lines = System.IO.File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                string[] values = line.Split("|");
                if (values[0] == "Id") continue;
                var station = new Station(values[1]);
                station.StationID = Convert.ToInt32(values[0]);
                station.EndStation = Convert.ToBoolean(values[2]);
                stations.Add(station);
            }

            return stations;
        }


    }
}
