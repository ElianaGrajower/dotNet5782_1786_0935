using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
        public struct Parcel
        {
            public int parcelId { set; get; }
            public int senderId { set; get; }
            public int targetId { set; get; } //the id of the customer who is receiving the parcel
            public weightCategories weight { set; get; } //light, average, heavy
            public Priorities priority { set; get; } //regular, fast, emergency
            public int droneId { set; get; }
            public DateTime? requested { set; get; } //The time the parcel is made
            public DateTime? scheduled { set; get; } //The time the parcel is matched up with a drone
            public DateTime? pickedUp { set; get; } //The time the drones picks up the parcel from the sender
            public DateTime? delivered { set; get; } //The time the customer receives the oarcel
            public bool fragile { set; get; } //if the parcel is fragile    //***should we take it out???

            public override string ToString()
            {
                return String.Format($"Parcel Id: {parcelId}\nSender Id: {senderId}\nTarget Id: {targetId}\nDrone Id: {droneId}\nweight: {weight}\npriority: {priority}\nrequested: { requested}\nscheduled: {scheduled}\nPicked Up: { pickedUp }\ndelivered: { delivered}");
            }
            


        }
    }

