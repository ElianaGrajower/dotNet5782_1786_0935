using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class StationDrone
    {
        public int DroneId;
        public int StationName;
        public int StationId;
        public int DroneBatteryPercentage;
        //timestamp
        public override string ToString()
        {
            return String.Format($"Drone Id: {DroneId}\nStation Name: {StationName}\nStation Id: {StationId}\nDrone Battery: {DroneBatteryPercentage}\n");
        }
    }
}
