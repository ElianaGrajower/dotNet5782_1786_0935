using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Station
    {
        public int StationId { set; get; }
        public int Name { set; get; }
        public double Longitude { set; get; }
        public double Lattitude { set; get; }
        public int ChargeSlots { set; get; } //the amount of available charge slots in the station
    }
}
