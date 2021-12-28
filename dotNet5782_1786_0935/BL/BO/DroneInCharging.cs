using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace BO
{
    public class DroneInCharging
    {
        public DateTime chargeTime;
        public int droneId;   
        public double battery; //in percentages


        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId}\nBattery: {battery}\n");
        }
    }
}
