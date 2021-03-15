﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TrainEngine
{
    public class Time
    {
        public void RunTime()
        {
            string time = "12:00";
            while (true)
            {
                string[] hourandminutes = time.Split(':');
                int hour = Convert.ToInt32(hourandminutes[0]);
                int minut = Convert.ToInt32(hourandminutes[1]);

                if (minut == 59)
                {
                    minut = 0;

                    if (hour == 23) hour = 0;
                    else hour++;
                }
                else minut++;

                string[] hourAndMinutesInts = { hour.ToString(), minut.ToString() };
                if (Convert.ToInt32(hourAndMinutesInts[1]) <= 9)
                    hourAndMinutesInts[1] = hourAndMinutesInts[1].Insert(0, "0");
                time = string.Join(':', hourAndMinutesInts);

                Thread.Sleep(1000);
                Console.WriteLine(time + " i RunTime");
            }
        }
    }
}
