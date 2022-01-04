using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updaetd and needed
namespace BO
{
    public class Drone
    {     
        public int droneId { set; get; }
        public string model { set; get; }
        public weightCategories maxWeight { set; get; }  //light, average, heavy
        public double battery { set; get; } //in percentages
        public DroneStatus droneStatus { set; get; }  //available, maintenance, delivery
        public ParcelInTransit parcel { set; get; }
        public Location location { set; get; }

        public override string ToString()
        {
            return String.Format($"Id: {droneId}\nModel: {model}\nMax weight: {maxWeight}\nBattery: {battery}%\nDrone Status: {droneStatus}\nParcel in Transit: {parcel}\nLocation: {location}\n");
        }
    }
}
