using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public class ParcelDrone
    {
        public int DroneId;
        public int ParcelId;
        public WeightCategories MaxDroneWeight;
        public WeightCategories ParcelWeight;
        public override string ToString()
        {
            return String.Format($"Drone Id: {DroneId}\nParcel Id: {ParcelId}\nMax Drone Weight: {MaxDroneWeight}\nParcel Weight: {ParcelWeight}\n");
        }
    }
}
