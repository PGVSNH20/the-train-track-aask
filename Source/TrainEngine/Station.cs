namespace TrainEngine
{
    public class Station
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
        public bool EndStation { get; set; }


        public Station(string name)
        {
            StationName = name;
        }
    }
}
