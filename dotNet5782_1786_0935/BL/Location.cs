﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace IBL.BO
{
    public class Location
    {
        public double Lattitude;
        public double Longitude;
        public Location(char a,double x,char b,double y,char c) 
        {
            Lattitude = x;
            Longitude = y;
        }
        public Location(double x,double y)
        {
            Lattitude = x;
            Longitude = y;
        }
        public override string ToString()
        {
            return String.Format($"Location: ( {Lattitude} , {Longitude} )\n");
        }
    }
}
