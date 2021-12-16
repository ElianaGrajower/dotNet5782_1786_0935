using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updated and needed
namespace IBL.BO
{
    public class Station
    {
        public int StationId { set; get; }     
        public string name { set; get; }
        public Location location { set; get; }
        public int chargeSlots { set; get; } //the amount of available charge slots in the station
        public void decreaseChargeSlots() { chargeSlots--; } //dcrease a charge slot
        public int numberOfSlotsInUse;
        public void addChargeSlots() { chargeSlots++; } //add a charge slot
        public List<DroneInCharging> DronesatStation; //drones being charged at the station
        public override string ToString()
        {
            return String.Format($"Station Id: {StationId}\nStation Name: {name}\nStation Location: {location}\nAvailable chargeSlots: {chargeSlots- numberOfSlotsInUse}\nnumberOfSlotsInUse: {numberOfSlotsInUse}");
        }
    }
}
