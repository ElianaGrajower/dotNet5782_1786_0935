using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
        public struct DroneCharge
        {
            public int droneId { set; get; } //id of the drone being charged
            public int stationId { set; get; } //id of the station that the drone is being charged at
            public DateTime chargeTime;
            public bool active { set; get; }
        public override string ToString()
            {
                return "Drone Id: " + droneId + "\n" + "Station Id: " + stationId;
            }
        }
    }

