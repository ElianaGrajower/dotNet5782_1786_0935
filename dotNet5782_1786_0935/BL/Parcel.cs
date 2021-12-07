﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL.BO
{
    class Parcel
    {
        public int ParcelId { set; get; }
        public int SenderId { set; get; }
        public int TargetId { set; get; } //the id of the customer who is receiving the parcel
        public WeightCategories Weight { set; get; } //light, average, heavy
        public Priorities Priority { set; get; } //regular, fast, emergency
        public int DroneId { set; get; }
        public DateTime Requested { set; get; } //The time the parcel is made
        public DateTime Scheduled { set; get; } //The time the parcel is matched up with a drone
        public DateTime PickedUp { set; get; } //The time the drones picks up the parcel from the sender
        public DateTime Delivered { set; get; } //The time the customer receives the oarcel
    }
}