using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DroneCharging
    {
        public int stationId;
        public int DroneId;   
        public double battery;
        public DateTime arrival;
        public override string ToString()
        {
            return String.Format($"Drone Id: {DroneId}\n Battery: {battery}\n Arrival time: {arrival}\n");
        }
    }
}
