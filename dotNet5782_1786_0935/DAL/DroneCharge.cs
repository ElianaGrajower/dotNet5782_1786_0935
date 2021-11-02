using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneId { set; get; } //id of the drone being charged
            public int StationId { set; get; } //id of the station that the drone is being charged at
            public override string ToString()
            {
                return "Drone Id: " + DroneId + "\n" + "Station Id: " + StationId;
            }
        }
    }
}
