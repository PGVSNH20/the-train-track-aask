using System.Collections.Generic;

namespace TrainEngine
{
    public interface ITravelPlan
    {
        public void Save(string path);
        public void Load(string path);

        public ITravelPlan StartAt(string station, string depTime);
        public ITravelPlan ArriveAt(string station, string arrTime);
        public ITravelPlan GeneratePlan();
    }
}
