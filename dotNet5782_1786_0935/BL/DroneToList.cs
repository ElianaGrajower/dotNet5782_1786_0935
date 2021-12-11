using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace IBL.BO
{
    public class DroneToList
    {
        public int droneId;
        public string Model { set; get; }
        public WeightCategories weight;
        public double battery;
        public DroneStatus droneStatus;
        public Location location;   
        public int parcelId;
        public int numOfParcelsDelivered;

        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId}\nModel: {Model}\nWeight: {weight}\nbattery: {battery}\nDrone Status: {droneStatus}\nLocation: {location}\nParcel Id: {parcelId}\nNumber Of Parcels Delivered: {numOfParcelsDelivered}\n");
        }
    }
}
