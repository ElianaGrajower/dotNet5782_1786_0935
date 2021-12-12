using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//asap fix the extra made up availabledronecharge
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
                availableChargeSlots = slots;

            }
            public int StationId { set; get; } 
            public int Name { set; get; }
            public double Lattitude { set; get; }
            public double Longitude { set; get; }
            public int ChargeSlots { set; get; }
            public int availableChargeSlots { set; get; }
            public override string ToString()
            {
                return String.Format($"Id: {StationId}\nName: {Name}\nLongitude: {Longitude}Lattitude: {Lattitude}\nChargeSlots: {ChargeSlots}");
            }
        }
    }
}
