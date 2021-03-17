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
            var crossing = (description.Parts[4] as Crossing);
            while (true)
            {
                if(time.Time == "12:06")
                {
                    crossing.barriersDown = true;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Carlos closed the barriers");
                    Console.ForegroundColor = ConsoleColor.White;
                }                
                
                if(time.Time == "12:08")
                {
                    crossing.barriersDown = false;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Carlos opened the barriers");
                    Console.ForegroundColor = ConsoleColor.White;
                }                
                
                if(time.Time == "12:10")
                {
                    crossing.barriersDown = true;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Carlos closed the barriers");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Thread.Sleep(500);
            } 
        }
    }
}
