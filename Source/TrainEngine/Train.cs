﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine
{
    public class Train
    {
        public string TrainName { get; set; }
        public int TrainID { get; set; }
        public int MaxSpeed { get; set; }
        public bool Operated { get; set; }

        public Train(string name)
        {
            TrainName = name;
        }
    
    }


}
