using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TrainEngine
{
    public class Signals
    {
        private Clock time;
        private TrackDescription description;
        public Signals(Clock time, TrackDescription description)
        {
            this.time = time;
            this.description = description;
        }

        public void simulate()
        {
            while(true)
            {
                if(time.Time == "12:06")
                {
                    (description.Parts[4] as Crossing).barriersDown = true;
                    Console.WriteLine("Bommar nere");
                }
                Thread.Sleep(500);
            } 
        }
    }
}
