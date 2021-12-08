using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace IBL.BO
{
    public class DroneInParcel
    {
        public int droneId;
        public double battery;
        public Location location;
   

        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId}\nBattery Percentage{battery}");
        }
    }
}
