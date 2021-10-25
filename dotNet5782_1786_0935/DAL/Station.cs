using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        class Station
        {
            int id { set; get; }
            int Name { set; get; }
            double Longitude { set; get; }
            double Lattitude { set; get; }
            int ChargeSlots { set; get; }
            public override string ToString()
            {
                return String.Format($"id:{id}\nName:{Name}\nLongitude:{Longitude}\nLattitude:{Lattitude}\nChargeSlots:{ChargeSlots}\n");
            }
        }
    }
}
