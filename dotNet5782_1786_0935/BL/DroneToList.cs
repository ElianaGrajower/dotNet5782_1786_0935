using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace IBL.BO
{
    class DroneToList
    {
        public int droneId;
        public string Model { set; get; }
        public WeightCategories weight;
        public double battery;
        public DroneStatus droneStatus;
        public Location location;
        public int parcelId;
        public int numOfParcelsDelivered;
    }
}
