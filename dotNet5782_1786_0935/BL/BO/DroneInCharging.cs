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
        public DateTime chargeTime { get; set; }
        public int droneId { get; set; }
        public double battery { get; set; } //in percentages
        public double chargeTillNow { get; set; }

        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId} Battery: {battery}\n");
        }
    }
}
