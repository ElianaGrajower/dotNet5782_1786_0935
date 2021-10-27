using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int id { set; get; }
            public int Name { set; get; }
            public double Longitude { set; get; }
            public double Lattitude { set; get; }
            public int ChargeSlots { set; get; }
            public override string ToString()
            {
                return String.Format($"id: {id}\nName: {Name}\nLongitude: {Longitude}\nLattitude: {Lattitude}\nChargeSlots: {ChargeSlots}\n");
            }
        }
    }
}
