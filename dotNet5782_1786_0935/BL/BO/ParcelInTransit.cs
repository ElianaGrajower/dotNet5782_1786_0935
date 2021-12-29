using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updaded and needed

namespace BO
{
    public class ParcelInTransit
    {
        public int parcelId { set; get; }
       // public bool parcelStatus; //true if matched with drone
        public Priorities priority;
        public weightCategories weight;
        public CustomerInParcel sender;   
        public CustomerInParcel target;
        public Location pickupLocation;
        public Location targetLocation;
        public double distance;
        public ParcelStatus parcelStatus;


        public override string ToString()
        {
            return String.Format($"Parcel Id: {parcelId} Parcel Status: {parcelStatus} priority: {priority} weight: {weight} Sender: {sender} Target: {target} Pickup Location: {pickupLocation} Target Location: {targetLocation} distance: {distance}\n");
        }
    }
}
