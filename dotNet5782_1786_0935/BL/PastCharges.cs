using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   public class PastCharges
    {
        public int DroneId;
        public DateTime arrival;
        public DateTime discharge;
        public override string ToString()
        {
            return String.Format($"Drone Id: {DroneId} \n Arrival Time: {arrival}\n Discharge time: {discharge}\n");
        }
    }
}
