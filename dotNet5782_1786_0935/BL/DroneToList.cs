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
        public string model { set; get; }
        public weightCategories weight; //light, average, heavy
        public double battery; //in percentages
        public DroneStatus droneStatus; //available, maintenance, delivery
        public Location location;   
        public int parcelId;
        public int numOfParcelsdelivered; //the number of parcels that the drone delivered

        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId} Model: {model} weight: {weight} battery: {battery}% Drone Status: {droneStatus} Location: {location} Parcel Id: {parcelId} Number Of Parcels delivered: {numOfParcelsdelivered}\n");
        }
    }
}
