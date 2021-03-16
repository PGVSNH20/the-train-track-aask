using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public class Train
    {
        public int TrainID { get; set; }
        public string TrainName { get; set; }
        public int MaxSpeed { get; set; }
        public bool Operated { get; set; }
        public bool Moving { get; set; } = false;

        public Train(string name)
        {
            TrainName = name;
        }
    
    }


}
