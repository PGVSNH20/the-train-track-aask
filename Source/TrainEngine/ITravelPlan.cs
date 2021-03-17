namespace TrainEngine
{
    public interface ITravelPlan
    {
        public void Save(string path);
        public ITravelPlan GeneratePlan();
        public void Simulate(Clock time);
        public ITravelPlan AddEntry(TimeTableEntry entry);
    }
}
