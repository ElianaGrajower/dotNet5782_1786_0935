using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updated and needed

namespace IBL.BO   
{
    public class Location
    {
        public double lattitude;
        public double longitude;
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
        public override string ToString()
        {
            return String.Format($" { Math.Round(lattitude,3)} °N, {Math.Round(longitude,3)} °W");
        }
    }
}
