using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace BO
{
    public class DroneToList
    {
        public int droneId { get; set; }
        public string model { set; get; }
        public weightCategories weight { get; set; } //light, average, heavy
        public double battery { get; set; } //in percentages
        public DroneStatus droneStatus { get; set; } //available, maintenance, delivery
        public Location location { get; set; }
        public int parcelId { get; set; }
        public int numOfParcelsdelivered { get; set; } //the number of parcels that the drone delivered

        public override string ToString()
        {
            return String.Format($"Drone Id: {droneId} Model: {model} weight: {weight} battery: {Math.Round(battery, 3)}% Drone Status: {droneStatus} Location: {location} Parcel Id: {parcelId} Number Of Parcels delivered: {numOfParcelsdelivered}\n");
        }
    }
}
