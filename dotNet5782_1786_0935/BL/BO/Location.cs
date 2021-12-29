using System;
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
        public double latitude { set; get; }
        public double longitude { set; get; }
        public Location(char a,double x,char b,double y,char c) 
        {
            latitude = x;
            longitude = y;
        }
        public Location(double x,double y)
        {
            latitude = x;
            longitude = y;
        }

        public Location()
        {
            ///added this on 29/12 not sure if correct it was empty before
            latitude = 30;
            longitude = 35;
           
        }
        public override string ToString()
        {
            return String.Format($" { Math.Round(latitude,3)} °N, {Math.Round(longitude,3)} °W");
        }
    }
}
