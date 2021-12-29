using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updated and needed
namespace BO
{
    public class Station
    {
        public int stationId { set; get; }     
        public string name { set; get; }
        public Location location { set; get; }
        public int chargeSlots { set; get; } //the amount of available charge slots in the station
        public void decreaseChargeSlots() { chargeSlots--; } //dcrease a charge slot
        public int numberOfSlotsInUse { set; get; }
      //  public void addChargeSlots() { chargeSlots++; } //add a charge slot
        public List<DroneInCharging> dronesAtStation; //drones being charged at the station
        public bool active;
        public override string ToString()
        {
            return String.Format($"Station Id: {stationId}\nStation name: {name}\nStation Location: {location}\nAvailable chargeSlots: {chargeSlots- numberOfSlotsInUse}\n Drones charging: {dronesAtStation}");
        }
    }
}
