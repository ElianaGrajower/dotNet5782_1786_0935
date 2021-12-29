using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace BO
{
    public class DroneInParcel
    {
        public int droneId { set; get; }
        public double battery { set; get; } //in percentages
        public Location location { set; get; }


        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId} Battery: {battery} Location: {location}\n");
        }
    }
}
