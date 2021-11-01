using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
       public struct Customer
        {
            public int id { set; get; }
            public string Name { set; get; }
            public string Phone { set; get; }
            public double Longitude { set; get; }
            public double Lattitude { set; get; }
            public override string ToString()
            {
                return String.Format($"id: {id}\nName: {Name}\nPhone: {Phone}\nLongitude: {Longitude}\nLattitude: {Lattitude}");
            }
        }
    }
}
