using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//uodated and needed

namespace IBL.BO
{
    public class Parcel
    {
        public int ParcelId { set; get; }
        public CustomerInParcel Sender { set; get; }
        public CustomerInParcel Target { set; get; } //the id of the customer who is receiving the parcel
        public WeightCategories Weight { set; get; } //light, average, heavy
        public Priorities Priority { set; get; } //regular, fast, emergency
        public DroneInParcel Drone { set; get; }   
        public DateTime Requested { set; get; } //The time the parcel is made
        public DateTime Scheduled { set; get; } //The time the parcel is matched up with a drone
        public DateTime PickedUp { set; get; } //The time the drones picks up the parcel from the sender
        public DateTime Delivered { set; get; } //The time the customer receives the parcel
     //   public bool Fragile { set; get; } //if the parcel is fragile    
        public override string ToString()
        {
            return String.Format($"Parcel Id: {ParcelId}\nSender: {Sender}\nTarget : {Target}\nDrone : {Drone}\nWeight: {Weight}\nPriority: {Priority}\nRequested: { Requested}\nScheduled: {Scheduled}\nPicked Up: { PickedUp }\nDelivered: { Delivered}\nFragile: {Fragile}");
        }
    }
}
