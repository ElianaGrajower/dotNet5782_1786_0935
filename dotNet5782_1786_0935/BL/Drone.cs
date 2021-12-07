using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace IBL.BO
{
    public class Drone
    {     
        public int DroneId { set; get; }
        public string Model { set; get; }
        public WeightCategories MaxWeight;
        public double MilesCovered;
        public List<StationDrone> ListofDroneStations; //a list of all the stations the drone charged at.
        public int DroneBatteryPercentage; //the battery that the drone has left in percentages out of 100%.//instructions
        public List<DroneInParcel> ListofDroneParcels; //a list of all the parcels that the drone carried.

        public override string ToString()
        {
            return String.Format($"Id: {DroneId}\nModel: {Model}\nMaxWeight: {MaxWeight}\n");
            //Status: {Status}\nBattery: {Battery}");
        }
    }
}
