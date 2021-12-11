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
        public WeightCategories ParcelWeight;
        public override string ToString()
        {
            return String.Format($"Parcel Id: {ParcelId}\n\nParcel Weight: {ParcelWeight}\n");
        }
    }
}
