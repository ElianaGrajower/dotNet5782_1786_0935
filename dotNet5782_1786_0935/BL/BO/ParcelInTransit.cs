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
        public Priorities priority { set; get; }
        public weightCategories weight { set; get; }
        public CustomerInParcel sender { set; get; }
        public CustomerInParcel target { set; get; }
        public Location pickupLocation { set; get; }
        public Location targetLocation { set; get; }
        public double distance { set; get; }
        public bool parcelStatus { set; get; }


        public override string ToString()
        {
            return String.Format($"Parcel Id: {parcelId} Parcel Status: {parcelStatus} priority: {priority} weight: {weight} Sender: {sender} Target: {target} Pickup Location: {pickupLocation} Target Location: {targetLocation} distance: {distance}\n");
        }
    }
}
