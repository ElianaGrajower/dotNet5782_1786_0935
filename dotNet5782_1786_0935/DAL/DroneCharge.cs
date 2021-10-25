using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace Do
    {
        public struct DroneCharge
        {
            int DroneId;
            int StationId;
            public override string ToString()
            {
                return "Drone Id: " + DroneId + "\n" + "Station Id: " + StationId + "\n";
            }
        }
    }
}
