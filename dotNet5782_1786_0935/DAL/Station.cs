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
            public Station(int id, int name, double lat, double lon, int slots)
            {
                StationId = id;
                Name = name;
                Lattitude=lat;
                Longitude = lon;
                ChargeSlots = slots;

            }
            public int StationId { set; get; } 
            public int Name { set; get; }
            public double Lattitude { set; get; }
            public double Longitude { set; get; }
            public int ChargeSlots { set; get; } //the amount of available charge slots in the station
            public override string ToString()
            {
                return String.Format($"Id: {StationId}\nName: {Name}\nLongitude: {Longitude}\nLattitude: {Lattitude}\nChargeSlots: {ChargeSlots}");
            }
        }
    }
}
