using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public interface ITravelPlan
    {
        public List<object> TimeTable { get; }

        public Train Train { get; }

        public void Save(string path);
        public void Load(string path);
    }
}
