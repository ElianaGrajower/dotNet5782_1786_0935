﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updated and needed

namespace BO   
{
    public class Location
    {
        public double lattitude { set; get; }
        public double longitude { set; get; }
        public Location(char a,double x,char b,double y,char c) 
        {
            lattitude = x;
            longitude = y;
        }
        public Location(double x,double y)
        {
            lattitude = x;
            longitude = y;
        }

        public Location()
        {
            //////what is supposed to go here??????
        }
        public override string ToString()
        {
            return String.Format($" { Math.Round(lattitude,3)} °N, {Math.Round(longitude,3)} °W");
        }
    }
}
