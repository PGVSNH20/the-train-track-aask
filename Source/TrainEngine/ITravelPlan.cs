using System.Collections.Generic;

namespace TrainEngine
{
    public interface ITravelPlan
    {
        public List<TimeTableEntry> TimeTable { get; set; }

        public Train Train { get; }

        public void Save(string path);
        public void Load(string path);
    }
}
