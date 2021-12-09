using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace IBL.BO
{
    public class DroneInCharging
    {
        public int droneId;
        public double battery;


        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId}\nBattery: {battery}\n");
        }
    }
}
