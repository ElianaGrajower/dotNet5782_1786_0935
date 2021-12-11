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
        public bool parcelStatus;//true if matched with drone
        public Priorities priority;
        public WeightCategories weight;
        public CustomerInParcel sender;
        public CustomerInParcel target;
        public Location pickupLocation;
        public Location targetLocation;
        public double distance;


        public override string ToString()
        {
            return String.Format($"Parcel Id: {parcelId}\nParcel Status: {parcelStatus}\nPriority: {priority}\nWeight: {weight}\nSender: {sender}\nTarget: {target}\nPickup Location: {pickupLocation}\nTarget Location: {targetLocation}\ndistance: {distance}\n");
        }
    }
}
