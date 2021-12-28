using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//asap fix the extra made up availabledronecharge

    namespace DO
    {
        public struct Station
        {
            public Station(int id, string tempname, double lat, double lon, int slots)
            {
                stationId = id;
                name = tempname;
                lattitude=lat;
                longitude = lon;
                chargeSlots = slots;
                active = true;
                
              //  availablechargeSlots = slots;

            }
            public int stationId { set; get; } 
            public string name { set; get; }
            public double lattitude { set; get; }
            public double longitude { set; get; }
            public int chargeSlots { set; get; }
            public bool active { set; get; }
        // public int availablechargeSlots { set; get; }
        public override string ToString()
            {
                return String.Format($"Id: {stationId}\nname: {name}\nlongitude: {longitude}lattitude: {lattitude}\nchargeSlots: {chargeSlots}");
            }
        }
    }

