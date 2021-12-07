using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updaded and needed

namespace IBL.BO
{
    public class ParcelInTransit
    {
        public int parcelId;
        public bool parcelStatus;
        public Priorities priority;
        public WeightCategories weight;
        public CustomerInParcel sender;
        public CustomerInParcel terget;
        public Location pickupLocation;
        public Location targetLocation;
        public double distance;


    }
}
