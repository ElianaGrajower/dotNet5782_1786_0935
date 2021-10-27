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
            public int DroneId { set; get; }
            public int StationId { set; get; }
            public override string ToString()
            {
                return "Drone Id: " + DroneId + "\n" + "Station Id: " + StationId + "\n";
            }
        }
    }
}
