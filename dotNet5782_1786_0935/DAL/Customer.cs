using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        class Customer
        {
            int id { set; get; }
            string Name { set; get; }
            string Phone { set; get; }
            double Longitude { set; get; }
            double Lattitude { set; get; }
            public override string ToString()
            {
                return String.Format($"id:{id}\nName:{Name}\nPhone:{Phone}\nLongitude:{Longitude}\nLattitude:{Lattitude}\n");
            }
        }
    }
}
