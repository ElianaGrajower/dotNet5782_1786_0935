using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//uodated and needed

namespace BO
{
    public class Parcel
    {
        public int parcelId { set; get; }
        public CustomerInParcel sender { set; get; } //the id of the customer who is sending the parcel
        public CustomerInParcel target { set; get; } //the id of the customer who is receiving the parcel
        public weightCategories weight { set; get; } //light, average, heavy
        public Priorities priority { set; get; } //regular, fast, emergency   
        public DroneInParcel drone { set; get; }   
        public DateTime? requested { set; get; } //The time the parcel is made
        public DateTime? scheduled { set; get; } //The time the parcel is matched up with a drone
        public DateTime? pickedUp { set; get; } //The time the drones picks up the parcel from the sender
        public DateTime? delivered { set; get; } //The time the customer receives the parcel
        public bool fragile { set; get; } //if the parcel is fragile    //******should we take it out?
        public bool active;
        public override string ToString()
        {
            return String.Format($"Parcel Id: {parcelId}\nSender: {sender}\nTarget: {target}\nweight: {weight}\npriority: {priority}\nDrone: {drone}\nrequested: {requested}\nscheduled: {scheduled}\nPicked Up: { pickedUp }\ndelivered: { delivered}\nfragile: {fragile}");
        }
    }
}
