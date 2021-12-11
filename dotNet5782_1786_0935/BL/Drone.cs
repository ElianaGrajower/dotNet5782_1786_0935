using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updaetd and needed
namespace IBL.BO
{
    public class Drone
    {     
        public int DroneId { set; get; }
        public string Model { set; get; }
        public WeightCategories MaxWeight;
        public double battery;
        public DroneStatus droneStatus { set; get; }   
        public ParcelInTransit parcel;
        public Location location;

        public override string ToString()
        {
            return String.Format($"Id: {DroneId}\nModel: {Model}\nMax Weight: {MaxWeight}\nBattery: {battery}\nDrone Status: {droneStatus}\nParcel in Transit: {parcel}\nLocation: {location}\n");
        }
    }
}
